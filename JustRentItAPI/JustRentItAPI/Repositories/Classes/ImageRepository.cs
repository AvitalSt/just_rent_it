using JustRentItAPI.Data;
using JustRentItAPI.Models.DTOs;
using JustRentItAPI.Models.Entities;
using JustRentItAPI.Repositories.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace JustRentItAPI.Repositories.Classes
{
    public class ImageRepository : IImageRepository
    {
        private readonly AppDbContext _context;
        private readonly Cloudinary _cloudinary;

        public ImageRepository(AppDbContext context, Cloudinary cloudinary)
        {
            _context = context;
            _cloudinary = cloudinary;
        }

        public async Task<DressImageDTO> UploadAsync(IFormFile file, int? dressId = null)
        {
            var logoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "logo-img.png");

            using var image = Image.Load(file.OpenReadStream());
            using var logo = Image.Load(logoPath);

            float logoScale = 0.50f;
            int logoWidth = (int)(image.Width * logoScale);
            int logoHeight = logo.Height * logoWidth / logo.Width;

            logo.Mutate(x => x.Resize(logoWidth, logoHeight));

            var position = new SixLabors.ImageSharp.Point((image.Width - logoWidth) / 2,
                (image.Height - logoHeight) / 2
            );

            image.Mutate(ctx =>
            {
                ctx.DrawImage(
                    logo,
                    position,
                    new GraphicsOptions
                    {
                        AlphaCompositionMode = PixelAlphaCompositionMode.SrcOver,
                        BlendPercentage = 0.55f
                    });
            });

            using var ms = new MemoryStream();
            await image.SaveAsJpegAsync(ms, new JpegEncoder { Quality = 70 });
            ms.Seek(0, SeekOrigin.Begin);

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(Guid.NewGuid().ToString(), ms),
                Folder = "JustRentIt/Dresses"
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.Error != null)
            {
                throw new Exception($"Cloudinary Upload Error: {uploadResult.Error.Message}");
            }

            DressImage? imageEntity = null;
            string finalPath = uploadResult.SecureUrl.ToString(); 

            if (dressId.HasValue)
            {
                imageEntity = new DressImage
                {
                    DressID = dressId.Value,
                    ImagePath = finalPath,
                    IsMain = false
                };
                _context.DressImages.Add(imageEntity);
                await _context.SaveChangesAsync();
            }

            return new DressImageDTO
            {
                ImageID = imageEntity?.DressImageID ?? 0,
                ImagePath = finalPath,
                IsMain = false
            };
        }

        public async Task<bool> DeleteAsync(int imageId)
        {
            var image = await _context.DressImages.FindAsync(imageId);
            if (image == null)
                return false;

            _context.DressImages.Remove(image);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}