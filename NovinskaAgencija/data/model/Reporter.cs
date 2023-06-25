namespace NovinskaAgencija.data.model
{
    public class Reporter
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string Ime { get; set; } = string.Empty;
        public string Prezime { get; set; } = string.Empty;
        //public ICollection<ReporterClanak> ReporterClanak { get; set; }
        public ICollection<Clanak> Clanak { get; set; }
    }
}
