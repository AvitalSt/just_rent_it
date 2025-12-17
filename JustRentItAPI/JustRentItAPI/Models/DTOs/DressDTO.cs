using JustRentItAPI.Models.Enums;

namespace JustRentItAPI.Models.DTOs
{
    public class DressDTO
    {
        //STRING מאתחלים אותם כי הם יכולות להיות "נאל" ואם נאתחל זה חוסך לי בדיקה
        //כל שאר השדות לא יכול להיות "נאל" יש להם ערך ברירת מחלדה 0 וכו
        public int DressID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Price { get; set; }
        public List<string> Colors { get; set; } = new();
        public List<string> Sizes { get; set; } = new();
        public List<string> Cities { get; set; } = new();
        public List<string> AgeGroups { get; set; } = new();
        public List<string> EventTypes { get; set; } = new();
        public SaleType SaleType { get; set; }
        public DressState State { get; set; }
        public DressStatus Status { get; set; }
        public string MainImage { get; set; } = string.Empty;
        public List<DressImageDTO> Images { get; set; } = new List<DressImageDTO>();
        public List<string> Categories { get; set; } = new();
        public bool IsFavorite { get; set; } = false;
        public int Views { get; set; }
        public int OwnerId { get; set; }
    }
}
