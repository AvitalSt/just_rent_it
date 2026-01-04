namespace JustRentItAPI.Models.DTOs
{
    public class MonthlySummarySendRequestDTO
    {
        public int DaysToSplit { get; set; } = 1;  
        public int DayIndex { get; set; } = 1;     
    }
}
