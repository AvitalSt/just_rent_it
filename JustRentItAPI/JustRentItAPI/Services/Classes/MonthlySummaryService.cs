using JustRentItAPI.Models.DTOs;
using JustRentItAPI.Models.Entities;
using JustRentItAPI.Repositories.Interfaces;
using JustRentItAPI.Services.Interfaces;
using System;
using System.Net;

namespace JustRentItAPI.Services.Classes
{
    public class MonthlySummaryService : IMonthlySummaryService
    {
        private readonly IInterestRepository _interestRepository;
        private readonly IMailService _mailService;
        private readonly IMonthlySummaryRepository _monthlySummaryRepository;
        private readonly string _frontendBaseUrl;

        public MonthlySummaryService(IInterestRepository interestRepository, IMailService mailService, IMonthlySummaryRepository monthlySummaryRepository, IConfiguration configuration)
        {
            _interestRepository = interestRepository;
            _mailService = mailService;
            _monthlySummaryRepository = monthlySummaryRepository;
            _frontendBaseUrl = configuration["FrontendUrl"];
        }

        public async Task<Response<MonthlySummaryLastDTO>> GetLastSummaryAsync()
        {
            try
            {
                var last = await _monthlySummaryRepository.GetLastAsync();

                return new Response<MonthlySummaryLastDTO>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Message = "Last monthly summary fetched successfully",
                    Data = new MonthlySummaryLastDTO
                    {
                        LastSent = last?.SentAt
                    }
                };
            }
            catch (Exception ex)
            {
                return new Response<MonthlySummaryLastDTO>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = ex.Message,
                };
            }
        }

        public async Task<Response> SendMonthlySummaryAsync()
        {
            try
            {
                //מחשבים את החודש הקודם
                /* var now = DateTime.UtcNow;
                 int month = now.Month == 1 ? 12 : now.Month - 1;
                 int year = now.Month == 1 ? now.Year - 1 : now.Year;

                 var from = new DateTime(year, month, 1);
                 var to = from.AddMonths(1);*/

                var now = DateTime.UtcNow;

                var from = new DateTime(now.Year, now.Month, 1);
                var to = from.AddMonths(1);

                List<Interest> interests = await _interestRepository.GetByDateRangeAsync(from, to);

                if (!interests.Any())
                {
                    return new Response
                    {
                        IsSuccess = true,
                        StatusCode = HttpStatusCode.OK,
                        Message = "No interests found for previous month."
                    };
                }

                await SendOwnerMonthlySummaryAsync(interests);
                await SendUserMonthlySummaryAsync(interests);

                await _monthlySummaryRepository.AddAsync(new MonthlySummary
                {
                    SentAt = DateTime.UtcNow
                });

                return new Response
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Message = "Monthly summary sent successfully."
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = ex.Message
                };
            }
        }

        private async Task SendOwnerMonthlySummaryAsync(List<Interest> interests)
        {
            // קיבוץ לפי בעלת השמלה
            var groupedByOwner = interests.GroupBy(i => i.Dress.UserID);

            foreach (var ownerGroup in groupedByOwner)
            {
                var owner = ownerGroup.First().Dress.User;
                var ownerName = owner.FirstName;
                var ownerEmail = owner.Email;

                // קיבוץ לפי שמלה -> רשימת מתעניינות
                var dressData = ownerGroup
                     .GroupBy(i => i.Dress)
                     .Select(g => (
                          DressName: g.Key.Name,
                          DressUrl: $"{_frontendBaseUrl}/dresses/{g.Key.DressID}",
                          InterestedNames: g
                            .Select(x => $"{x.User.FirstName} {x.User.LastName}")
                            .Distinct()//שלא יופיע שם פעמים
                            .ToList()
                    ))
                    .ToList();

                if (dressData.Any())
                    await _mailService.SendOwnerMonthlySummaryAsync(owner.Email, owner.FirstName, dressData);
            }
        }

        private async Task SendUserMonthlySummaryAsync(List<Interest> interests)
        {
            var groupedByUser = interests.GroupBy(i => i.UserID);

            foreach (var userGroup in groupedByUser)
            {
                var user = userGroup.First().User;
                var userName = user.FirstName;
                var userEmail = user.Email;

                var dressData = userGroup
                    .Select(i => (Name: i.Dress.Name, Url: $"{_frontendBaseUrl}/dresses/{i.Dress.DressID}"))
                    .Distinct()
                    .ToList();

                if (dressData.Any())
                    await _mailService.SendUserMonthlySummaryAsync(user.Email, user.FirstName, dressData);
            }
        }
    }
}

