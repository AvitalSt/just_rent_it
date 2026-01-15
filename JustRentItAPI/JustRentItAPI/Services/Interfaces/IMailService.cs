using JustRentItAPI.Models.DTOs;
using JustRentItAPI.Models.Entities;

namespace JustRentItAPI.Services.Interfaces
{
    public interface IMailService
    {
        Task<Response> SendEmailAsync(string toEmail, string subject, string body, string? fromEmail = null);

        Task SendDressDeletedAsync(string email, string firstName, string dressName);

        Task SendDressActivatedAsync(string email, string firstName, string dressName, int dressId);


        Task SendOwnerFollowUpAsync(string ownerEmail, string ownerName, string interestedName,string dressName, int dressId);

        Task SendUserFollowUpAsync(string userEmail, string userName, string dressName, int dressId);

        Task SendPaymentAsync(string ownerEmail, string ownerName);

        Task SendUserInterestAsync(string userEmail, string userFirstName, string dressName, int dressId, string ownerFirstName, string ownerLastName, string ownerEmail, string ownerPhone);

        Task SendOwnerInterestAsync(string ownerEmail, string ownerFirstName, string userFirstName, string userLastName, string userEmail, string userPhone, string dressName, int dressId, string? message);


        Task SendOwnerMonthlySummaryAsync(string ownerEmail, string ownerName, List<(string DressName, string DressUrl, List<string> InterestedNames)> dresses);

        Task SendUserMonthlySummaryAsync(string userEmail, string userName, List<(string Name, string Url)> dresses);


        Task<Response> SendPasswordResetEmailAsync(string email, string firstName, string resetLink);
    }
}
