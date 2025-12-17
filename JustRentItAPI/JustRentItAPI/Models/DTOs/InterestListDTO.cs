namespace JustRentItAPI.Models.DTOs
{
    public class InterestListDTO
    {
        public int InterestID { get; set; }
        public int DressID { get; set; }
        public string DressName { get; set; } = string.Empty;
        public int OwnerID { get; set; }
        public string OwnerName { get; set; } = string.Empty;
        public string OwnerEmail { get; set; } = string.Empty;
        public int UserID { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public DateTime SentDate { get; set; }
        public string Status { get; set; } = "Pending";
        public int PaymentAmount { get; set; }
        public string Notes { get; set; } = string.Empty;
        public string OwnerComment { get; set; } = string.Empty;
        public string UserComment { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public int OwnerMailCount { get; set; } = 0;
        public int UserMailCount { get; set; } = 0;
    }
}

