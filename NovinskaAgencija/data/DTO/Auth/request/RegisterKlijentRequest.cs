namespace NovinskaAgencija.data.DTO.Auth.request
{
    public class RegisterKlijentRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string StateOfOrigin { get; set; } = string.Empty;
        public string JurassicAccount { get; set; } = string.Empty;
        public string NazivKompanije { get; set; } = string.Empty;
        public string TipPreduzeca { get; set; } = string.Empty;
    }
}
