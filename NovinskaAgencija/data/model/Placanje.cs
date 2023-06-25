namespace NovinskaAgencija.data.model
{
    public class Placanje
    {
        public int Id { get; set; }
        public DateTime TransactionDate { get; set; }
        public int ClanakId { get; set; }
        public Clanak Clanak { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
