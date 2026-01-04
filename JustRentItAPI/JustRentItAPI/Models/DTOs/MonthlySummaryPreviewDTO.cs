namespace JustRentItAPI.Models.DTOs
{
    public class MonthlySummaryPreviewDTO
    {
        public DateTime FromUtc { get; set; }
        public DateTime ToUtc { get; set; }
        public int OwnerEmails { get; set; }
        public int UserEmails { get; set; }
        public int TotalEmails => OwnerEmails + UserEmails;
    }
}
