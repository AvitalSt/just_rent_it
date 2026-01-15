using JustRentItAPI.Models.DTOs;
using JustRentItAPI.Models.Entities;
using JustRentItAPI.Repositories.Interfaces;
using JustRentItAPI.Services.Interfaces;
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

        public async Task<Response<MonthlySummaryPreviewDTO>> PreviewMonthlySummaryAsync()
        {
            try
            {
                var now = DateTime.UtcNow;
                var (from, to) = GetPreviousMonthRangeUtc(now);

                var interests = await _interestRepository.GetByDateRangeAsync(from, to);

                var ownerEmails = interests
                    .Select(i => i.Dress.UserID)
                    .Distinct()
                    .Count();

                var userEmails = interests
                    .Select(i => i.UserID)
                    .Distinct()
                    .Count();

                return new Response<MonthlySummaryPreviewDTO>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Data = new MonthlySummaryPreviewDTO
                    {
                        FromUtc = from,
                        ToUtc = to,
                        OwnerEmails = ownerEmails,
                        UserEmails = userEmails
                    },
                    Message = $"Preview for {from:MM/yyyy} ready."
                };
            }
            catch (Exception ex)
            {
                return new Response<MonthlySummaryPreviewDTO>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = ex.Message
                };
            }
        }

        public async Task<Response> SendMonthlySummaryAsync(int daysToSplit, int dayIndex)
        {
            try
            {
                if (daysToSplit < 1) daysToSplit = 1;
                if (dayIndex < 1) dayIndex = 1;
                if (dayIndex > daysToSplit) dayIndex = daysToSplit;

                var now = DateTime.UtcNow;
                var (from, to) = GetPreviousMonthRangeUtc(now);

                var interests = await _interestRepository.GetByDateRangeAsync(from, to);

                if (!interests.Any())
                {
                    return new Response
                    {
                        IsSuccess = true,
                        StatusCode = HttpStatusCode.OK,
                        Message = $"No interests found for {from:MM/yyyy}."
                    };
                }

                var ownersToday = interests
                    .Where(i => IsInDayPart(i.Dress.UserID, daysToSplit, dayIndex))
                    .ToList();

                var usersToday = interests
                    .Where(i => IsInDayPart(i.UserID, daysToSplit, dayIndex))
                    .ToList();

                await SendOwnerMonthlySummaryAsync(ownersToday);
                await SendUserMonthlySummaryAsync(usersToday);

                await _monthlySummaryRepository.AddAsync(new MonthlySummary { SentAt = DateTime.UtcNow });

                return new Response
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Message = $"Sent day {dayIndex}/{daysToSplit} for {from:MM/yyyy}."
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

        //פונציה טהורה מחזירה תמיד tuple
        private static (DateTime from, DateTime to) GetPreviousMonthRangeUtc(DateTime nowUtc)
        {
            int year = nowUtc.Month == 1 ? nowUtc.Year - 1 : nowUtc.Year;
            int month = nowUtc.Month == 1 ? 12 : nowUtc.Month - 1;

            var from = new DateTime(year, month, 1, 0, 0, 0, DateTimeKind.Utc);
            int lastDay = DateTime.DaysInMonth(year, month);
            var to = new DateTime(year, month, lastDay, 23, 59, 59, DateTimeKind.Utc);

            return (from, to);
        }

        private static bool IsInDayPart(int userId, int daysToSplit, int dayIndex1Based)
        {
            if (daysToSplit <= 1) return true;
            int part = Math.Abs(userId) % daysToSplit; 
            return part == (dayIndex1Based - 1);
        }

        private async Task SendOwnerMonthlySummaryAsync(List<Interest> interests)
        {
            var groupedByOwner = interests.GroupBy(i => i.Dress.UserID);

            foreach (var ownerGroup in groupedByOwner)
            {
                var owner = ownerGroup.First().Dress.User;
                var ownerName = owner.FirstName;
                var ownerEmail = owner.Email;

                var dressData = ownerGroup
                     .GroupBy(i => i.Dress)
                     .Select(g => (
                          DressName: g.Key.Name,
                          DressUrl: $"{_frontendBaseUrl}/dresses/{g.Key.DressID}",
                          InterestedNames: g
                            .Select(x => $"{x.User.FirstName} {x.User.LastName}")
                            .Distinct()
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

