using NovinskaAgencija.data.DTO.Article.response;
using NovinskaAgencija.data.model;

namespace NovinskaAgencija.data.DTO.Auth.response
{
    public class LoginReporterResponse
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? ImageUrl { get; set; } = string.Empty;
        public Role Role { get; set; }
        public string StateOfOrigin { get; set; } = string.Empty;
        public string JurassicAccount { get; set; } = string.Empty;
        public List<ArticleResponse> Clanci { get; set; }
        public List<PlacanjaResponse> Placanja { get; set; }
        public string Token { get; set; } = string.Empty;
        public string Ime { get; set; } = string.Empty;
        public string Prezime { get; set; } = string.Empty;
        public bool IsVerified { get; set; }
    }
}
