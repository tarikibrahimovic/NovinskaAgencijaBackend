namespace NovinskaAgencija.data.DTO.User.request
{
    public class ChangePasswordRequest
    {
        public string Password { get; set; } = string.Empty;
        public string OldPassword { get; set; } = string.Empty;
    }
}
