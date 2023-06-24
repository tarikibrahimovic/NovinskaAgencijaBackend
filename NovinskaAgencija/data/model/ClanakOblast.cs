namespace NovinskaAgencija.data.model
{
    public class ClanakOblast
    {
        public int Id { get; set; }
        public int ClanakId { get; set; }
        public Clanak Clanak { get; set; }
        public int OblastId { get; set; }
        public Oblast Oblast { get; set; }
    }
}
