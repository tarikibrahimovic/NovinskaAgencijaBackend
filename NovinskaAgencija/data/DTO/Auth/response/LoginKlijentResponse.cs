using NovinskaAgencija.data.DTO.Article.response;
using NovinskaAgencija.data.model;

namespace NovinskaAgencija.data.DTO.Auth.response
{
    public class LoginKlijentResponse
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? ImageUrl { get; set; } = string.Empty;
        public Role Role { get; set; }
        public string StateOfOrigin { get; set; } = string.Empty;
        public string JurassicAccount { get; set; } = string.Empty;
        public List<PlacanjaResponse> Placanje { get; set; }
        public string Token { get; set; } = string.Empty;
        public string NazivKompanije { get; set; } = string.Empty;
        public string TipPreduzeca { get; set; }
        public bool IsVerified { get; set; }
    }
}
