namespace NovinskaAgencija.data.model
{
    public class ReporterClanak
    {
        public int Id { get; set; }
        public int ClanakId { get; set; }
        public Clanak Clanak { get; set; }
        public int ReporterId { get; set; }
        public Reporter Reporter { get; set; }
    }
}
