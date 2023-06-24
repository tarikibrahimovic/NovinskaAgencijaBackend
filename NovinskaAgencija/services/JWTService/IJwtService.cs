using NovinskaAgencija.data.model;

namespace NovinskaAgencija.services.JWTService
{
    public interface IJwtService
    {
        public string CreateToken(User user);
        public int GetUserId(string token);
    }
}
