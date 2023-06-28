namespace NovinskaAgencija.data.DTO.Article.response
{
    public class PlacanjaResponse
    {
        public int Id { get; set; }
        public DateTime TransactionDate { get; set; }
        public int ClanakId { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
