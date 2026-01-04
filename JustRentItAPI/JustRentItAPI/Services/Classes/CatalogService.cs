/*using JustRentItAPI.Models.Entities;
using JustRentItAPI.Repositories.Interfaces;
using JustRentItAPI.Services.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System.Net;
using System.Text;


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
        *//*        private readonly string _catalogPath;
        *//*
        private readonly string _templatesPath;
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

            _baseUrl = config["ApiBaseUrl"];
            *//*            _catalogPath = Path.Combine(_env.WebRootPath, "catalog.pdf");
            */
/*            _templatesPath = Path.Combine(_env.WebRootPath, "catalog");
*//*
_config = config;
_httpClientFactory = httpClientFactory;
}

*//*  private string LoadTemplate(string fileName)
  {
      var full = Path.Combine(_templatesPath, fileName);
      return File.ReadAllText(full);
  }*/

/*        private async Task<byte[]> GeneratePdfFromHtml(string html)
        {
            var exePath = Environment.GetEnvironmentVariable("PUPPETEER_EXECUTABLE_PATH");

            var launchOptions = new LaunchOptions
            {
                Headless = true,
                ExecutablePath = string.IsNullOrWhiteSpace(exePath) ? null : exePath,
                IgnoreHTTPSErrors = true,
                Args = new[]
                {
                    "--no-sandbox",
                    "--disable-setuid-sandbox",
                    "--disable-dev-shm-usage",
                    "--disable-gpu",
                    "--ignore-certificate-errors",
                    "--force-device-scale-factor=1"
                }
            };

            await using var browser = await Puppeteer.LaunchAsync(launchOptions);
            await using var page = await browser.NewPageAsync();

            page.DefaultTimeout = 300000;
            page.DefaultNavigationTimeout = 300000;

            await page.SetViewportAsync(new ViewPortOptions
            {
                Width = 794,
                Height = 1123,
                DeviceScaleFactor = 1
            });

            await page.EmulateMediaTypeAsync(MediaType.Print);
            await page.SetContentAsync(html, new NavigationOptions
            {
                WaitUntil = new[] { WaitUntilNavigation.Load }
            });

            await page.WaitForFunctionAsync(
                    @"() => {
                const imgs = Array.from(document.images);
                return imgs.length === 0 || imgs.every(img => img.complete);
            }",
                new WaitForFunctionOptions
                {
                    Timeout = 300000
                }
            );

            return await page.PdfDataAsync(new PdfOptions
            {
                Format = PaperFormat.A4,
                PrintBackground = true,
                MarginOptions = new MarginOptions
                {
                    Top = "10px",
                    Bottom = "10px",
                    Left = "10px",
                    Right = "10px"
                }
            });
        }
*//*

public async Task<JustGenericResponse> GenerateCatalogAsync()
{
    try
    {
        QuestPDF.Settings.License = LicenseType.Community;

        var dresses = await _dressRepository.GetAllForCatalogAsync();

        if (dresses == null || !dresses.Any())
            return new JustGenericResponse { IsSuccess = false, Message = "לא נמצאו שמלות לקטלוג." };

        var pdfBytes = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(1, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.ContentFromRightToLeft();

                page.Content().Column(col =>
                {
                    col.Item().PaddingTop(100).AlignCenter().Width(220).Image(GetImageFromUrlAsync($"{_baseUrl}logo-img.png"));
                    col.Item().PaddingTop(40).AlignCenter().Text("קטלוג השמלות של JustRentIt").ExtraBold().FontSize(42);
                    col.Item().PaddingTop(20).AlignCenter().Text("מצאת את השמלה המושלמת?\nרוצה פרטים נוספים או להשכיר?").FontSize(24).AlignCenter();

                    col.Item().PaddingTop(30).Background(Colors.Black).Padding(20).Column(innerCol =>
                    {
                        innerCol.Item().AlignCenter().Text("ניתן לפנות אלינו במייל:").FontColor(Colors.White).FontSize(22);
                        innerCol.Item().AlignCenter().Text("info@justrentitdress.com").FontColor(Colors.White).ExtraBold().FontSize(28);
                    });

                    col.Item().PaddingTop(30).AlignCenter().Text("נא לצרף בפנייה:").FontSize(24).Bold();
                    col.Item().PaddingRight(50).Text("• צילום של השמלה\n• שם מלא\n• טלפון ליצירת קשר\n• כתובת מייל").FontSize(22).LineHeight(1.5f);
                });
            });

            var chunks = dresses
                .Select((d, i) => new { Index = i, Value = d })
                .GroupBy(x => x.Index / 12)
                .Select(g => g.Select(x => x.Value).ToList())
                .ToList();

            int globalIndex = 1;

            foreach (var dressChunk in chunks)
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(10, Unit.Millimetre);
                    page.ContentFromRightToLeft();

                    page.Footer().PaddingTop(5).Row(row =>
                    {
                        row.RelativeItem().Text(text =>
                        {
                            text.DefaultTextStyle(x => x.FontSize(10).FontColor(Colors.Grey.Medium));

                            text.Span("עמוד ");
                            text.CurrentPageNumber();
                            text.Span(" מתוך ");
                            text.TotalPages();
                        });

                        row.ConstantItem(80).AlignRight().Image(GetImageFromUrl($"{_baseUrl}logo-img.png"));
                    });

                    page.Content().PaddingVertical(10).Grid(grid =>
                    {
                        grid.VerticalSpacing(15);
                        grid.HorizontalSpacing(12);
                        grid.Columns(4);

                        foreach (var d in dressChunk)
                        {
                            grid.Item().Border(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(8).Column(col =>
                            {
                                col.Item().Row(row => row.RelativeItem().AlignRight().Text(globalIndex.ToString()).FontSize(10).Bold());

                                var mainImage = d.Images.FirstOrDefault(img => img.IsMain)?.ImagePath ?? d.Images.FirstOrDefault()?.ImagePath;
                                var imgUrl = mainImage?.Contains("http") == true ? mainImage : $"{_baseUrl.TrimEnd('/')}/{mainImage?.TrimStart('/')}";

                                if (imgUrl.Contains("cloudinary"))
                                    imgUrl = imgUrl.Replace("/upload/", "/upload/w_300,h_450,c_fill,q_auto,f_jpg/");

                                var imgData = GetImageFromUrl(imgUrl);
                                if (imgData.Length > 0)
                                    col.Item().Height(170).Image(imgData);

                                col.Item().PaddingTop(5).AlignCenter().Text(d.Name).Bold().FontSize(9).LineHeight(1.1f);
                                col.Item().AlignCenter().Text($"{d.Price} ₪").FontSize(9).Bold();
                            });
                            globalIndex++;
                        }
                    });
                });
            }
        }).GeneratePdf();

        return new JustGenericResponse { IsSuccess = true, Data = pdfBytes, StatusCode = HttpStatusCode.OK };
    }
    catch (Exception ex)
    {
        return new JustGenericResponse { IsSuccess = false, Message = "שגיאה ביצירת הקטלוג: " + ex.Message };
    }
}

private async Task<byte[]> GetImageFromUrlAsync(string url)
{
    try
    {
        var client = _httpClientFactory.CreateClient();
        return await client.GetByteArrayAsync(url);
    }
    catch
    {
        return Array.Empty<byte>();
    }
}

*//*public async Task<JustGenericResponse> GenerateCatalogAsync()
{
    try
    {
        var dresses = await _dressRepository.GetAllForCatalogAsync();

        var html = BuildHtml(dresses);

        var pdf = await GeneratePdfFromHtml(html);

        return new JustGenericResponse
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Message = "הקטלוג נוצר.",
            Data = pdf
        };
    }
    catch (Exception ex)
    {
        return new JustGenericResponse
        {
            IsSuccess = false,
            StatusCode = HttpStatusCode.InternalServerError,
            Message = "שגיאה ביצירת קטלוג: " + ex.Message
        };
    }
}*/

/*    public async Task<JustResponse> SaveCatalogAsync(byte[] pdf)
    {
        try
        {
            await File.WriteAllBytesAsync(_catalogPath, pdf);

            return new JustResponse
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Message = "הקטלוג נשמר בהצלחה."
            };
        }
        catch (Exception ex)
        {
            return new JustResponse
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.InternalServerError,
                Message = "שגיאה בשמירת הקטלוג: " + ex.Message
            };
        }
    }
*//*

    public async Task<JustResponse> SaveCatalogAsync(byte[] pdf)
    {
        try
        {
            using var ms = new MemoryStream(pdf);
            ms.Position = 0;

            var uploadParams = new RawUploadParams
            {
                File = new FileDescription("catalog.pdf", ms),
                PublicId = "catalog/latest",
                Overwrite = true,
                Invalidate = true,
                AccessMode = "public"
            };

            await _cloudinary.UploadAsync(uploadParams);

            return new JustResponse
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Message = "הקטלוג נשמר בענן."
            };
        }
        catch (Exception ex)
        {
            return new JustResponse
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.InternalServerError,
                Message = "שגיאה בשמירת הקטלוג: " + ex.Message
            };
        }
    }

    public string GetCatalogUrl()
    {
        var cloudName = _config["CloudinarySettings:CLOUDINARY_CLOUD_NAME"];
        long version = DateTime.UtcNow.Ticks;
        return $"https://res.cloudinary.com/{cloudName}/raw/upload/catalog/latest.pdf?v={version}";
    }

 *//*   private string BuildHtml(List<Dress> dresses)
    {
        var css = LoadTemplate("catalog.css");
        var cover = LoadTemplate("cover.html").Replace("{BASE_URL}", _baseUrl);
        var footer = LoadTemplate("footer.html").Replace("{BASE_URL}", _baseUrl);

        var pages = BuildDressPages(dresses);

        var sb = new StringBuilder();

        sb.Append("<html dir='rtl'><head><meta charset='UTF-8'/>");
        sb.Append("<style>" + css + "</style>");
        sb.Append("</head><body>");

        sb.Append(footer);
        sb.Append(cover);
        sb.Append(pages);

        sb.Append("</body></html>");

        return sb.ToString();
    }

    private string BuildDressPages(List<Dress> dresses)
    {
        var pageTemplate = LoadTemplate("page-template.html");

        var sb = new StringBuilder();
        int index = 1;

        for (int i = 0; i < dresses.Count; i += 12)
        {
            var cardsHtml = new StringBuilder();

            for (int j = 0; j < 12; j++)
            {
                int idx = i + j;
                if (idx >= dresses.Count)
                {
                    cardsHtml.Append("<div></div>");
                    continue;
                }

                var d = dresses[idx];
                var mainImage = d.Images.FirstOrDefault(img => img.IsMain)
                                ?? d.Images.FirstOrDefault();

                var imgPath = mainImage?.ImagePath ?? "Uploads/default.png";
                var fullImg = imgPath.Contains("http") ? imgPath : $"{_baseUrl.TrimEnd('/')}/{imgPath.TrimStart('/')}";

                cardsHtml.Append($@"
                    <div class='card'>
                        <div class='number'>{index}</div>
                        <img src='{fullImg}'/>
                        <div class='name'>{d.Name}</div>
                        <div class='price'>{d.Price} ₪</div>
                    </div>");

                index++;
            }

            var pageHtml = pageTemplate.Replace("{CARDS}", cardsHtml.ToString());
            sb.Append(pageHtml);
        }

        return sb.ToString();
    }*//*
}
}*/


using JustRentItAPI.Models.Entities;
using JustRentItAPI.Repositories.Interfaces;
using JustRentItAPI.Services.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System.Net;
using System.Text;

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
        }

        public async Task<JustGenericResponse> GenerateCatalogAsync()
        {
            try
            {
                Console.WriteLine("--- Starting Catalog Generation ---");
                QuestPDF.Settings.License = LicenseType.Community;

                var dresses = await _dressRepository.GetAllForCatalogAsync();
                if (dresses == null || !dresses.Any())
                    return new JustGenericResponse { IsSuccess = false, Message = "לא נמצאו שמלות לקטלוג." };

                Console.WriteLine($"Found {dresses.Count} dresses. Starting image downloads...");

                using var semaphore = new SemaphoreSlim(10);
                var logoTask = GetImageFromUrlAsync($"{_baseUrl}logo-img.png");

                int completed = 0;
                var imageTasks = dresses.Select(async d =>
                {
                    await semaphore.WaitAsync();
                    try
                    {
                        var mainImage = d.Images.FirstOrDefault(img => img.IsMain)?.ImagePath ?? d.Images.FirstOrDefault()?.ImagePath;
                        if (string.IsNullOrEmpty(mainImage)) return new { Id = d.DressID, Bytes = Array.Empty<byte>() };

                        var imgUrl = mainImage.Contains("http") ? mainImage : $"{_baseUrl}{mainImage.TrimStart('/')}";

                        // אופטימיזציה קריטית של גודל התמונה לפני שהיא מגיעה לשרת שלך
                        if (imgUrl.Contains("cloudinary"))
                            imgUrl = imgUrl.Replace("/upload/", "/upload/w_250,h_350,c_limit,q_auto:low,f_jpg/");

                        var bytes = await GetImageFromUrlAsync(imgUrl);

                        // לוג התקדמות כל 20 שמלות כדי לא להציף את הלוגים
                        var count = Interlocked.Increment(ref completed);
                        if (count % 20 == 0) Console.WriteLine($"Downloaded {count}/{dresses.Count} images...");

                        return new { Id = d.DressID, Bytes = bytes };
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                });

                var imagesResults = await Task.WhenAll(imageTasks);
                var logoBytes = await logoTask;
                var imageDict = imagesResults.ToDictionary(x => x.Id, x => x.Bytes);

                Console.WriteLine("All images downloaded. Creating PDF document...");

                // יצירת ה-PDF
                var pdfBytes = Document.Create(container =>
                {
                    // דף שער
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(1, Unit.Centimetre);
                        page.PageColor(Colors.White);
                        page.ContentFromRightToLeft();

                        page.Content().Column(col =>
                        {
                            if (logoBytes.Length > 0)
                                col.Item().PaddingTop(100).AlignCenter().Width(220).Image(logoBytes);

                            col.Item().PaddingTop(40).AlignCenter().Text("קטלוג השמלות של JustRentIt").ExtraBold().FontSize(42).FontColor(Colors.Black);
                            col.Item().PaddingTop(20).AlignCenter().Text("מצאת את השמלה המושלמת?\nרוצה פרטים נוספים או להשכיר?").FontSize(24).AlignCenter();

                            col.Item().PaddingTop(30).Background(Colors.Black).Padding(20).Column(innerCol =>
                            {
                                innerCol.Item().AlignCenter().Text("ניתן לפנות אלינו במייל:").FontColor(Colors.White).FontSize(22);
                                innerCol.Item().AlignCenter().Text("info@justrentitdress.com").FontColor(Colors.White).ExtraBold().FontSize(28);
                            });

                            col.Item().PaddingTop(30).AlignCenter().Text("נא לצרף בפנייה:").FontSize(24).Bold();
                            col.Item().PaddingRight(50).Text("• צילום של השמלה\n• שם מלא\n• טלפון ליצירת קשר\n• כתובת מייל").FontSize(22).LineHeight(1.5f);
                        });
                    });

                    // חלוקת השמלות לדפים (12 בכל דף)
                    var chunks = dresses
                        .Select((d, i) => new { Index = i, Value = d })
                        .GroupBy(x => x.Index / 12)
                        .Select(g => g.Select(x => x.Value).ToList());

                    int globalIndex = 1;

                    foreach (var dressChunk in chunks)
                    {
                        container.Page(page =>
                        {
                            page.Size(PageSizes.A4);
                            page.Margin(10, Unit.Millimetre);
                            page.ContentFromRightToLeft();

                            // פוטר לכל דף
                            page.Footer().PaddingTop(5).Row(row =>
                            {
                                row.RelativeItem().Text(text =>
                                {
                                    text.DefaultTextStyle(x => x.FontSize(10).FontColor(Colors.Grey.Medium));
                                    text.Span("עמוד ");
                                    text.CurrentPageNumber();
                                    text.Span(" מתוך ");
                                    text.TotalPages();
                                });

                                if (logoBytes.Length > 0)
                                    row.ConstantItem(80).AlignRight().Image(logoBytes);
                            });

                            page.Content().PaddingVertical(10).Grid(grid =>
                            {
                                grid.VerticalSpacing(15);
                                grid.HorizontalSpacing(12);
                                grid.Columns(4); // 4 עמודות

                                foreach (var d in dressChunk)
                                {
                                    grid.Item().Border(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(8).Column(col =>
                                    {
                                        col.Item().Row(row => row.RelativeItem().AlignRight().Text(globalIndex.ToString()).FontSize(10).Bold());

                                        if (imageDict.TryGetValue(d.DressID, out var imgData) && imgData.Length > 0)
                                        {
                                            col.Item().Height(170).Image(imgData);
                                        }
                                        else
                                        {
                                            col.Item().Height(170).Placeholder(); // במקרה שאין תמונה
                                        }

                                        col.Item().PaddingTop(5).AlignCenter().Text(d.Name).Bold().FontSize(9).LineHeight(1.1f);
                                        col.Item().AlignCenter().Text($"{d.Price} ₪").FontSize(9).Bold().FontColor(Colors.Blue.Medium);
                                    });
                                    globalIndex++;
                                }
                            });
                        });
                    }
                }).GeneratePdf();

                Console.WriteLine($"PDF generated successfully. Size: {pdfBytes.Length / 1024 / 1024} MB");

                // ניקוי זיכרון אקטיבי
                imageDict.Clear();

                return new JustGenericResponse { IsSuccess = true, Data = pdfBytes, StatusCode = HttpStatusCode.OK };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FATAL ERROR in GenerateCatalog: {ex.Message}");
                return new JustGenericResponse { IsSuccess = false, Message = "שגיאה ביצירת הקטלוג: " + ex.Message };
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
                    Invalidate = true,
                    AccessMode = "public"
                };

                await _cloudinary.UploadAsync(uploadParams);

                return new JustResponse { IsSuccess = true, StatusCode = HttpStatusCode.OK, Message = "הקטלוג נשמר בענן." };
            }
            catch (Exception ex)
            {
                return new JustResponse { IsSuccess = false, StatusCode = HttpStatusCode.InternalServerError, Message = "שגיאה בשמירת הקטלוג: " + ex.Message };
            }
        }

        public string GetCatalogUrl()
        {
            var cloudName = _config["CloudinarySettings:CLOUDINARY_CLOUD_NAME"];
            long version = DateTime.UtcNow.Ticks;
            return $"https://res.cloudinary.com/{cloudName}/raw/upload/catalog/latest.pdf?v={version}";
        }

        private async Task<byte[]> GetImageFromUrlAsync(string url)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                // הגדרת Timeout כדי שלא יתקע את כל התהליך אם שרת התמונות איטי
                client.Timeout = TimeSpan.FromSeconds(15);
                return await client.GetByteArrayAsync(url);
            }
            catch
            {
                return Array.Empty<byte>();
            }
        }

        public async Task<JustResponse> UpdateAndSaveCatalogAsync()
        {
            // 1. יצירת ה-PDF (כאן נוצר מערך הבייטים הגדול בזיכרון)
            var pdfResponse = await GenerateCatalogAsync();

            if (!pdfResponse.IsSuccess || pdfResponse.Data == null)
            {
                return new JustResponse
                {
                    IsSuccess = false,
                    Message = pdfResponse.Message,
                    StatusCode = pdfResponse.StatusCode
                };
            }

            // 2. שמירה ל-Cloudinary
            var saveResponse = await SaveCatalogAsync(pdfResponse.Data);

            // 3. שחרור הזיכרון - חשוב מאוד ל-1,000 שמלות!
            // אנחנו מוחקים את הנתונים מהאובייקט לפני שהוא חוזר לקונטרולר
            pdfResponse.Data = null;

            // 4. מחזירים רק תשובת סטטוס (בלי ה-PDF עצמו)
            return new JustResponse
            {
                IsSuccess = saveResponse.IsSuccess,
                Message = saveResponse.IsSuccess ? "הקטלוג עודכן ונשמר בהצלחה בשרת הענן" : saveResponse.Message,
                StatusCode = saveResponse.StatusCode
            };
        }
    }
}