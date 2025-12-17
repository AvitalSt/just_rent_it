using JustRentItAPI.Data;
using JustRentItAPI.Models.DTOs;
using JustRentItAPI.Models.Entities;
using JustRentItAPI.Repositories.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.Fonts;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;


namespace JustRentItAPI.Repositories.Classes
{
    public class ImageRepository : IImageRepository
    {
        private readonly AppDbContext _context;
        //Directory.GetCurrentDirectory() מחזיר את התיקייה בה האפלקציה רצה כרגע
        //Path.Combine מחבר את הנתיב של התיקייה עם שם התיקייה 
        private readonly string _imageFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

        public ImageRepository(AppDbContext context)
        {
            _context = context;
            //בודק אם התיקייה קיימת, אם לא קיימת יוצר אותה
            if (!Directory.Exists(_imageFolder))
                Directory.CreateDirectory(_imageFolder);
        }

        public async Task<DressImageDTO> UploadAsync(IFormFile file, int? dressId = null)
        {
            //Guid.NewGuid() מזהה ייחודי- שלא יחזור על עצמו לעולם
            var fileName = $"{Guid.NewGuid()}.jpg";
            //יוצר את הנתיב של התיקייה עם הקובץ
            var filePath = Path.Combine(_imageFolder, fileName);
            var logoPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "logo-img.png");

            using var image = Image.Load(file.OpenReadStream());

            //איכות תמונה 70 אחוז
            var encoder = new JpegEncoder { Quality = 70 };

            using var logo = Image.Load(logoPath);

            //חישוב גודל הלוגו בהתאם לתמונה
            float logoScale = 0.50f;
            int logoWidth = (int)(image.Width * logoScale);
            int logoHeight = logo.Height * logoWidth / logo.Width;

            // שינוי גודל הלוגו
            logo.Mutate(x => x.Resize(logoWidth, logoHeight));

            // מיקום במרכז
            var position = new Point(
                (image.Width - logoWidth) / 2,
                (image.Height - logoHeight) / 2
            );

            image.Mutate(ctx =>
            {
                ctx.DrawImage(
                    logo,
                    position,
                    new GraphicsOptions
                    {
                        AlphaCompositionMode = PixelAlphaCompositionMode.SrcOver,//שכבה מעל התמונה
                        BlendPercentage = 0.55f//נותן שקיפטצ
                    });
            });

            await image.SaveAsync(filePath, encoder);

            DressImage? imageEntity = null;
            if (dressId.HasValue)
            {
                imageEntity = new DressImage
                {
                    DressID = dressId.Value,
                    ImagePath = $"/Uploads/{fileName}",
                    IsMain = false
                };
                _context.DressImages.Add(imageEntity);
                await _context.SaveChangesAsync();
            }

            return new DressImageDTO
            {
                ImageID = imageEntity?.DressImageID ?? 0,
                ImagePath = $"/Uploads/{fileName}",
                IsMain = false
            };
        }

        public async Task<bool> DeleteAsync(int imageId)
        {
            var image = await _context.DressImages.FindAsync(imageId);
            if (image == null)
                return false;

            var cleanPath = image.ImagePath.TrimStart('/');
            var filePath = Path.Combine(_imageFolder, cleanPath);

            if (File.Exists(filePath))
                File.Delete(filePath);

            _context.DressImages.Remove(image);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
