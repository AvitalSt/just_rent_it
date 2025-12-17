using JustRentItAPI.Models.DTOs;
using JustRentItAPI.Models.Entities;

namespace JustRentItAPI.Services.Interfaces
{
    public interface IMailService
    {
        Task<Response> SendEmailAsync(string toEmail, string subject, string body);

        Task SendDressDeletedAsync(Dress dress);

        Task SendDressActivatedAsync(Dress dress, string baseUrl);



        Task SendOwnerFollowUpAsync(string ownerEmail, string ownerName, string interestedName,string dressName);

        Task SendUserFollowUpAsync(string userEmail, string userName, string dressName);

        Task SendPaymentAsync(string ownerEmail, string ownerName);

        Task SendUserInterestAsync(User owner, User user, Dress dress);

        Task SendOwnerInterestAsync(User owner, User user, Dress dress, string? message);


        Task SendOwnerMonthlySummaryAsync(string ownerEmail, string ownerName, List<(string DressName, List<string> InterestedNames)> dressData);

        Task SendUserMonthlySummaryAsync(string userEmail, string userName, List<string> dressNames);


        Task<Response> SendPasswordResetEmailAsync(User user, string resetLink);
    }
}
