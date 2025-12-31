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
                var now = DateTime.UtcNow;
                DateTime from;
                DateTime to;

                var fromDate = now.AddDays(-5);
                from = new DateTime(fromDate.Year, fromDate.Month, fromDate.Day, 0, 0, 0, DateTimeKind.Utc);
                to = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59, DateTimeKind.Utc);
/*
                int prevMonth = now.Month == 1 ? 12 : now.Month - 1;
                int prevYear = now.Month == 1 ? now.Year - 1 : now.Year;

                if (now.Day <= 15)
                {
                    from = new DateTime(prevYear, prevMonth, 1, 0, 0, 0, DateTimeKind.Utc);
                    to = new DateTime(prevYear, prevMonth, 15, 23, 59, 59, DateTimeKind.Utc);
                }
                else
                {
                    from = new DateTime(prevYear, prevMonth, 16, 0, 0, 0, DateTimeKind.Utc);
                    int lastDay = DateTime.DaysInMonth(prevYear, prevMonth);
                    to = new DateTime(prevYear, prevMonth, lastDay, 23, 59, 59, DateTimeKind.Utc);
                }*/

                List<Interest> interests = await _interestRepository.GetByDateRangeAsync(from, to);

                if (!interests.Any())
                {
                    return new Response
                    {
                        IsSuccess = true,
                        StatusCode = HttpStatusCode.OK,
                        Message = $"No interests found for the period {from:dd/MM} to {to:dd/MM}."
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
                    Message = $"Summary for {from:dd/MM/yyyy} - {to:dd/MM/yyyy} sent successfully."
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

