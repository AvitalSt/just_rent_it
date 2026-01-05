using JustRentItAPI.Models.Entities;
using JustRentItAPI.Repositories.Interfaces;
using JustRentItAPI.Services.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System.Net;
using QuestPDF.Drawing;

using JustResponse = JustRentItAPI.Models.DTOs.Response;
using JustGenericResponse = JustRentItAPI.Models.DTOs.Response<byte[]>;

namespace JustRentItAPI.Services.Classes
{
    public class CatalogService : ICatalogService
    {
        private readonly IDressRepository _dressRepository;
        private readonly IWebHostEnvironment _env;
        private readonly Cloudinary _cloudinary;
        private readonly IConfiguration _config;
        private readonly string _baseUrl;
        private readonly IHttpClientFactory _httpClientFactory;
        private static bool _fontRegistered = false;

        public CatalogService(
            IDressRepository dresses,
            IWebHostEnvironment env,
            IConfiguration config,
            Cloudinary cloudinary,
            IHttpClientFactory httpClientFactory)
        {
            _dressRepository = dresses;
            _env = env;
            _cloudinary = cloudinary;
            _config = config;
            _httpClientFactory = httpClientFactory;
            _baseUrl = config["ApiBaseUrl"]?.TrimEnd('/') + "/";
            RegisterHebrewFont();
        }

        public async Task<JustGenericResponse> GenerateCatalogAsync()
        {
            try
            {
                QuestPDF.Settings.License = LicenseType.Community;
                var dresses = await _dressRepository.GetAllForCatalogAsync();

                if (dresses == null || !dresses.Any())
                {
                    return new JustGenericResponse
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.NotFound,
                        Message = "לא נמצאו שמלות לקטלוג."
                    };
                }

                var httpClient = _httpClientFactory.CreateClient();
                httpClient.Timeout = TimeSpan.FromSeconds(30);

                var logoBytes = await GetImageFromUrlAsync(httpClient, $"{_baseUrl}logo-img.png");

                using var semaphore = new SemaphoreSlim(10);
                var imageTasks = dresses.Select(async d =>
                {
                    await semaphore.WaitAsync();
                    try
                    {
                        var path = d.Images.FirstOrDefault(i => i.IsMain)?.ImagePath ?? d.Images.FirstOrDefault()?.ImagePath;
                        var url = GetCloudinaryThumbnailUrl(path);
                        return new { Id = d.DressID, Bytes = await GetImageFromUrlAsync(httpClient, url) };
                    }
                    finally { semaphore.Release(); }
                });

                var imagesResults = await Task.WhenAll(imageTasks);
                var imageDict = imagesResults.ToDictionary(x => x.Id, x => x.Bytes);

                var pdfBytes = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.DefaultTextStyle(x => x.FontFamily("Heebo"));
                        page.Size(PageSizes.A4);
                        page.Margin(1, Unit.Centimetre);
                        page.ContentFromRightToLeft();

                        page.Content().Column(col =>
                        {
                            if (logoBytes.Length > 0)
                                col.Item().PaddingTop(10).AlignCenter().Width(180).Image(logoBytes);

                            col.Item().PaddingTop(20).AlignCenter().Text("קטלוג השמלות של JustRentIt")
                                .ExtraBold().FontSize(36).FontColor(Colors.Black);

                            col.Item().PaddingTop(10).AlignCenter().Text(text =>
                            {
                                text.Span("מצאת את השמלה המושלמת?\n").FontSize(22);
                                text.Span("רוצה פרטים נוספים או להשכיר?").FontSize(22);
                            });

                            col.Item().PaddingTop(25).Row(row =>
                            {
                                row.RelativeItem();
                                row.ConstantItem(350).Background(Colors.Black).Padding(20).Column(inner =>
                                {
                                    inner.Item().AlignCenter().Text("ניתן לפנות אלינו במייל:").FontColor(Colors.White).FontSize(20);
                                    inner.Item().AlignCenter().Text("info@justrentitdress.com").FontColor(Colors.White).ExtraBold().FontSize(24);
                                });
                                row.RelativeItem();
                            });

                            col.Item().PaddingTop(20).AlignCenter().Text("נא לצרף בפנייה:").FontSize(22).Bold();

                            col.Item().PaddingTop(10).PaddingRight(100).Column(list =>
                            {
                                string[] items = { "צילום של השמלה", "שם מלא", "טלפון ליצירת קשר", "כתובת מייל" };
                                foreach (var item in items)
                                {
                                    list.Item().Text($"• {item}").FontSize(20).LineHeight(1.2f);
                                }
                            });
                        });
                    });

                    var chunks = dresses.Select((d, i) => new { d, i }).GroupBy(x => x.i / 16);

                    foreach (var chunk in chunks)
                    {
                        container.Page(page =>
                        {
                            page.DefaultTextStyle(x => x.FontFamily("Heebo"));
                            page.Size(PageSizes.A4);
                            page.Margin(0.8f, Unit.Centimetre);
                            page.ContentFromRightToLeft();

                            page.Content().PaddingVertical(5).Grid(grid =>
                            {
                                grid.Columns(4);
                                grid.VerticalSpacing(15);
                                grid.HorizontalSpacing(10);

                                foreach (var item in chunk)
                                {
                                    grid.Item().Column(col =>
                                    {
                                        if (imageDict.TryGetValue(item.d.DressID, out var b) && b.Length > 0)
                                            col.Item().Height(145).Image(b).FitArea();
                                        else
                                            col.Item().Height(145).Placeholder();

                                        col.Item().PaddingTop(2).AlignCenter().Text(item.d.Name).Bold().FontSize(9);
                                        col.Item().AlignCenter().Text($"{item.d.Price} ₪").FontSize(10).Bold().FontColor(Colors.Black);
                                    });
                                }
                            });
                        });
                    }
                }).GeneratePdf();

                return new JustGenericResponse { IsSuccess = true, StatusCode = HttpStatusCode.OK, Data = pdfBytes };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Catalog Generation Error: {ex}");
                return new JustGenericResponse
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = "שגיאה פנימית ביצירת הקטלוג."
                };
            }
        }

        public async Task<JustResponse> SaveCatalogAsync(byte[] pdf)
        {
            try
            {
                using var ms = new MemoryStream(pdf);
                var uploadParams = new RawUploadParams
                {
                    File = new FileDescription("catalog.pdf", ms),
                    PublicId = "catalog/latest",
                    Overwrite = true,
                    Invalidate = true
                };

                var result = await _cloudinary.UploadAsync(uploadParams);

                if (result.Error != null)
                {
                    return new JustResponse
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = result.Error.Message
                    };
                }

                return new JustResponse { IsSuccess = true, StatusCode = HttpStatusCode.OK };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Cloudinary Upload Error: {ex}");
                return new JustResponse
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = "שגיאה בשמירת הקטלוג לענן."
                };
            }
        }

        public string GetCatalogUrl()
        {
            var cloudName = _config["CloudinarySettings:CLOUDINARY_CLOUD_NAME"];
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            return $"https://res.cloudinary.com/{cloudName}/raw/upload/catalog/latest.pdf?v={timestamp}";
        }

        private async Task<byte[]> GetImageFromUrlAsync(HttpClient client, string url)
        {
            try
            {
                using var request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get, url);
                request.Headers.Accept.ParseAdd("image/*");
                var response = await client.SendAsync(request);

                if (!response.IsSuccessStatusCode) return Array.Empty<byte>();
                return await response.Content.ReadAsByteArrayAsync();
            }
            catch { return Array.Empty<byte>(); }
        }

        private string GetCloudinaryThumbnailUrl(string? path)
        {
            if (string.IsNullOrEmpty(path)) return "";
            var url = path.StartsWith("http", StringComparison.OrdinalIgnoreCase) ? path : $"{_baseUrl}{path.TrimStart('/')}";

            if (url.Contains("/upload/") && url.Contains("res.cloudinary.com"))
                return url.Replace("/upload/", "/upload/w_300,h_450,c_fill,g_auto,q_auto:good,f_jpg/");

            return url;
        }

        private void RegisterHebrewFont()
        {
            if (_fontRegistered) return;
            try
            {
                var fontPath = Path.Combine(_env.ContentRootPath, "Assets", "Fonts", "Heebo-VariableFont_wght.ttf");
                if (File.Exists(fontPath))
                {
                    using var fontStream = File.OpenRead(fontPath);
                    FontManager.RegisterFont(fontStream);
                    _fontRegistered = true;
                }
            }
            catch (Exception ex) { Console.WriteLine($"Font error: {ex.Message}"); }
        }

        public async Task<JustResponse> UpdateAndSaveCatalogAsync()
        {
            var gen = await GenerateCatalogAsync();
            if (!gen.IsSuccess) return new JustResponse { IsSuccess = false, StatusCode = gen.StatusCode, Message = gen.Message };
            return await SaveCatalogAsync(gen.Data!);
        }
    }
}