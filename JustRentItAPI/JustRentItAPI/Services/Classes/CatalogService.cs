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
                    return new JustGenericResponse { IsSuccess = false, Message = "לא נמצאו שמלות לקטלוג." };

                var logoBytes = await GetImageFromUrlAsync($"{_baseUrl}logo-img.png");

                using var semaphore = new SemaphoreSlim(10);
                var imageTasks = dresses.Select(async d =>
                {
                    await semaphore.WaitAsync();
                    try
                    {
                        var path = d.Images.FirstOrDefault(i => i.IsMain)?.ImagePath ?? d.Images.FirstOrDefault()?.ImagePath;
                        var url = GetCloudinaryThumbnailUrl(path);
                        return new { Id = d.DressID, Bytes = await GetImageFromUrlAsync(url) };
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
                        page.Margin(1, Unit.Centimetre); // צמצום שוליים כללי
                        page.ContentFromRightToLeft();

                        page.Content().Column(col =>
                        {
                            if (logoBytes.Length > 0)
                                col.Item().PaddingTop(10).AlignCenter().Width(180).Image(logoBytes);

                            col.Item().PaddingTop(20).AlignCenter().Text("קטלוג השמלות של JustRentIt")
                                .ExtraBold().FontSize(36).FontColor(Colors.Black);

                            col.Item().PaddingTop(10).AlignCenter().Text(text => {
                                text.Span("מצאת את השמלה המושלמת?\n").FontSize(22);
                                text.Span("רוצה פרטים נוספים או להשכיר?").FontSize(22);
                            });

                            col.Item().PaddingTop(25).Row(row => {
                                row.RelativeItem();
                                row.ConstantItem(350).Background(Colors.Black).Padding(20).Column(inner => {
                                    inner.Item().AlignCenter().Text("ניתן לפנות אלינו במייל:").FontColor(Colors.White).FontSize(20);
                                    inner.Item().AlignCenter().Text("info@justrentitdress.com").FontColor(Colors.White).ExtraBold().FontSize(24);
                                });
                                row.RelativeItem();
                            });

                            col.Item().PaddingTop(20).AlignCenter().Text("נא לצרף בפנייה:").FontSize(22).Bold();

                            col.Item().PaddingTop(10).PaddingRight(100).Column(list => {
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
                            page.Margin(1, Unit.Centimetre);
                            page.ContentFromRightToLeft();

                            page.Content().PaddingVertical(10).Grid(grid =>
                            {
                                grid.Columns(4);
                                grid.VerticalSpacing(20);
                                grid.HorizontalSpacing(15);

                                foreach (var item in chunk)
                                {
                                    // הסרת Border ו-Padding כדי שהתמונה תהיה נקייה ותיכנס בקלות
                                    grid.Item().Column(col =>
                                    {
                                        if (imageDict.TryGetValue(item.d.DressID, out var b) && b.Length > 0)
                                            col.Item().Height(160).Image(b).FitArea();
                                        else
                                            col.Item().Height(160).Placeholder();

                                        col.Item().PaddingTop(4).AlignCenter().Text(item.d.Name).Bold().FontSize(9).LineHeight(1);

                                        // שינוי המחיר לשחור (Black)
                                        col.Item().AlignCenter().Text($"{item.d.Price} ₪").FontSize(10).Bold().FontColor(Colors.Black);
                                    });
                                }
                            });
                        });
                    }
                }).GeneratePdf();

                return new JustGenericResponse { IsSuccess = true, Data = pdfBytes, StatusCode = HttpStatusCode.OK };
            }
            catch (Exception ex)
            {
                return new JustGenericResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<JustResponse> UpdateAndSaveCatalogAsync()
        {
            var gen = await GenerateCatalogAsync();
            if (!gen.IsSuccess) return new JustResponse { IsSuccess = false, Message = gen.Message };
            return await SaveCatalogAsync(gen.Data);
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
                return new JustResponse { IsSuccess = result.Error == null, StatusCode = HttpStatusCode.OK };
            }
            catch (Exception ex) { return new JustResponse { IsSuccess = false, Message = ex.Message }; }
        }

        public string GetCatalogUrl()
        {
            var cloudName = _config["CloudinarySettings:CLOUDINARY_CLOUD_NAME"];
            return $"https://res.cloudinary.com/{cloudName}/raw/upload/v{DateTime.UtcNow.Ticks}/catalog/latest.pdf";
        }

        private async Task<byte[]> GetImageFromUrlAsync(string url)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                client.Timeout = TimeSpan.FromSeconds(20);
                return await client.GetByteArrayAsync(url);
            }
            catch { return Array.Empty<byte>(); }
        }

        private string GetCloudinaryThumbnailUrl(string? path)
        {
            if (string.IsNullOrEmpty(path)) return "";
            var url = path.Contains("http") ? path : $"{_baseUrl}{path.TrimStart('/')}";
            if (url.Contains("cloudinary"))
                return url.Replace("/upload/", "/upload/w_300,h_450,c_fill,g_auto,q_auto:good,f_jpg/");
            return url;
        }

        private void RegisterHebrewFont()
        {
            try
            {
                var fontPath = Path.Combine(_env.ContentRootPath, "Assets", "Fonts", "Heebo-VariableFont_wght.ttf");
                if (File.Exists(fontPath))
                {
                    using var fontStream = File.OpenRead(fontPath);
                    FontManager.RegisterFont(fontStream);
                }
            }
            catch (Exception ex) { Console.WriteLine($"Font error: {ex.Message}"); }
        }
    }
}