namespace NovinskaAgencija.data.model
{
    public enum Role
    {
        Reporter,
        Client,
        Admin
    }
    public class User
    {
        public int Id { get; set; }
        public bool IsActive { get; set; } = true;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public byte[]? PasswordHash { get; set; } = new byte[32];
        public byte[]? PassswordSalt { get; set; } = new byte[32];
        public string? ImageUrl { get; set; } = string.Empty;
        public string? VerificationToken { get; set; } = string.Empty;
        public string? ForgotPasswordToken { get; set; } = string.Empty;
        public Role Role { get; set; }
        public string StateOfOrigin { get; set; } = string.Empty;
        public string JurassicAccount { get; set; } = string.Empty;
        public ICollection<Placanje> Placanja { get; set; }
    }
}
