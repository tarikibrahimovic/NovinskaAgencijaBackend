namespace NovinskaAgencija.data.DTO.Auth.request
{
    public class RegisterReporterRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string StateOfOrigin { get; set; } = string.Empty;
        public string JurassicAccount { get; set; } = string.Empty;
        public string Ime { get; set; } = string.Empty;
        public string Prezime { get; set; } = string.Empty;
    }
}
