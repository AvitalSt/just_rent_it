using JustRentItAPI.Models.DTOs;
using JustRentItAPI.Models.Entities;
using JustRentItAPI.Models.Enums;

namespace JustRentItAPI.Mappers
{
    public static class InterestMapper
    {
        public static Interest FromCreateDto(CreateInterestDTO dto, int userId)
        {
            return new Interest
            {
                DressID = dto.DressID,
                UserID = userId,
                Message = dto.Message,
                SentDate = DateTime.UtcNow,
                Status = RentStatus.Pending,
                PaymentAmount = 0,
                Notes = string.Empty,
                OwnerComment = string.Empty,
                UserComment = string.Empty
            };
        }

        public static InterestListDTO ToListDTO(Interest i)
        {
            return new InterestListDTO
            {
                InterestID = i.InterestID,
                DressID = i.DressID,
                DressName = i.Dress.Name,
                OwnerID = i.Dress.UserID,
                OwnerName = $"{i.Dress.User.FirstName} {i.Dress.User.LastName}",
                OwnerEmail = i.Dress.User.Email,
                UserID = i.UserID,
                UserName = $"{i.User.FirstName} {i.User.LastName}",
                UserEmail = i.User.Email,
                SentDate = i.SentDate,
                Status = i.Status.ToString(),
                PaymentAmount = (int)i.PaymentAmount,
                Notes = i.Notes,
                OwnerComment = i.OwnerComment,
                UserComment = i.UserComment,
                Message = i.Message,
                OwnerMailCount = i.OwnerMailCount,
                UserMailCount = i.UserMailCount
            };
        }

        public static List<InterestListDTO> ToListDTOs(IEnumerable<Interest> interests)
        {
            return interests.Select(ToListDTO).ToList();
        }
    }
}
