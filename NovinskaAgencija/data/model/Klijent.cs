namespace NovinskaAgencija.data.model
{
    public class Klijent
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string NazivKompanije { get; set; } = string.Empty;
        public string TipPreduzeca { get; set; } = string.Empty;
    }
}
