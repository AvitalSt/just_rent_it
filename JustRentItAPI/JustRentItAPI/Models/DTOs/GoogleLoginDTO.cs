namespace JustRentItAPI.Models.DTOs
{
    public class GoogleLoginDTO
    {
        public string IdToken { get; set; } = string.Empty;
        public bool RememberMe { get; set; }
    }
}
