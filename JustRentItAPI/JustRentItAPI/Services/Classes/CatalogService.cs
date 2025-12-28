using JustRentItAPI.Models.Entities;
using JustRentItAPI.Repositories.Interfaces;
using JustRentItAPI.Services.Interfaces;
using PuppeteerSharp;
using PuppeteerSharp.Media;
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

        private readonly string _baseUrl;
        private readonly string _catalogPath;
        private readonly string _templatesPath;

        public CatalogService(
            IDressRepository dresses,
            IWebHostEnvironment env,
            IConfiguration config)
        {
            _dressRepository = dresses;
            _env = env;

            _baseUrl = config["ApiBaseUrl"];
            _catalogPath = Path.Combine(_env.WebRootPath, "catalog.pdf");

            _templatesPath = Path.Combine(_env.WebRootPath, "catalog");
        }

        private string LoadTemplate(string fileName)
        {
            var full = Path.Combine(_templatesPath, fileName);
            return File.ReadAllText(full);
        }

        private async Task<byte[]> GeneratePdfFromHtml(string html)
        {
            var launchOptions = new LaunchOptions
            {
                Headless = true,
                ExecutablePath = Environment.GetEnvironmentVariable("PUPPETEER_EXECUTABLE_PATH") ?? "/usr/bin/chromium",
                Args = new[]
                {
                    "--no-sandbox",
                    "--disable-setuid-sandbox",
                    "--disable-dev-shm-usage", 
                    "--disable-gpu"           
                }
            };

            await using var browser = await Puppeteer.LaunchAsync(launchOptions);

            using var page = await browser.NewPageAsync();

            await page.SetContentAsync(html);
            await page.WaitForSelectorAsync("img");
            await page.WaitForNetworkIdleAsync();

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

        public async Task<JustGenericResponse> GenerateCatalogAsync()
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
        }

        public async Task<JustResponse> SaveCatalogAsync(byte[] pdf)
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


        public async Task<byte[]> GetCatalogAsync()
        {
            if (!File.Exists(_catalogPath))
                return Array.Empty<byte>();

            return await File.ReadAllBytesAsync(_catalogPath);
        }

        private string BuildHtml(List<Dress> dresses)
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

            for (int i = 0; i < dresses.Count; i += 16)
            {
                var cardsHtml = new StringBuilder();

                for (int j = 0; j < 16; j++)
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
                    imgPath = imgPath.TrimStart('/');
                    var fullImg = $"{_baseUrl}{imgPath}";

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
        }
    }
}
