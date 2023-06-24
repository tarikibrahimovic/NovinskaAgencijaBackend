namespace NovinskaAgencija.data.DTO.Auth.request
{
    public class RefreshTokenRequest
    {
        public string Token { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
