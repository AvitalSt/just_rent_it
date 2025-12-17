using JustRentItAPI.Models.DTOs;
using JustRentItAPI.Models.Entities;

namespace JustRentItAPI.Mappers
{
    public static class DressMapper
    {
        public static DressListDTO MapToListDTO(Dress dress, List<int> userFavoriteDressIds)
        {
            return new DressListDTO
            {
                DressID = dress.DressID,
                Name = dress.Name,
                Price = dress.Price,
                MainImage = dress.MainImage,
                IsFavorite = userFavoriteDressIds.Contains(dress.DressID)
            };
        }

        public static DressListDTO MapToPublicListDTO(Dress dress)
        {
            return new DressListDTO
            {
                DressID = dress.DressID,
                Name = dress.Name,
                Price = dress.Price,
                MainImage = dress.MainImage,
                IsFavorite = false
            };
        }

        public static DressDTO MapToDetailedDTO(Dress dress, List<int> userFavoriteDressIds)
        {
            return new DressDTO
            {
                DressID = dress.DressID,
                Name = dress.Name,
                Description = dress.Description,
                Price = dress.Price,
                Colors = dress.Colors?
                .Select(dc => dc.Color?.NameHebrew ?? string.Empty)
                .Where(n => !string.IsNullOrWhiteSpace(n))
                .ToList()
                ?? new List<string>(),

                Sizes = dress.Sizes?
                .Select(ds => ds.Size?.Name ?? string.Empty)
                .Where(n => !string.IsNullOrWhiteSpace(n))
                .ToList()
                ?? new List<string>(),

                Cities = dress.Cities?
                .Select(dc => dc.City?.NameHebrew ?? string.Empty)
                .Where(n => !string.IsNullOrWhiteSpace(n))
                .ToList()
                ?? new List<string>(),

                AgeGroups = dress.AgeGroups?
                .Select(da => da.AgeGroup?.NameHebrew ?? string.Empty)
                .Where(n => !string.IsNullOrWhiteSpace(n))
                .ToList()
                ?? new List<string>(),

                EventTypes = dress.EventTypes?
                .Select(de => de.EventType?.NameHebrew ?? string.Empty)
                .Where(n => !string.IsNullOrWhiteSpace(n))
                .ToList()
                ?? new List<string>(),
                Images = dress.Images.Select(img => new DressImageDTO
                {
                    ImageID = img.DressImageID,
                    ImagePath = img.ImagePath,
                    IsMain = img.IsMain
                }).ToList(),
                SaleType = dress.SaleType,
                State = dress.State,
                Status = dress.Status,
                MainImage = dress.MainImage,
                //מיפוי של רישמת התמונה של השמלה לרשימה פשוטה של נתיבים 
                //השורה בודקת - אם השמלה הנוכחית נמצאת ברשימת השמלות שהמשתמש סימן כמועדפות
                IsFavorite = userFavoriteDressIds.Contains(dress.DressID),
                Views = dress.Views,
                OwnerId = dress.UserID
            };
        }
    }
}
