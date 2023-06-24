namespace NovinskaAgencija.data.model
{
    public class Clanak
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string? ImageUrl { get; set; } = string.Empty;
        public int Cena { get; set; }
        public string FileUrl { get; set; } = string.Empty;
        public ICollection<ClanakOblast> ClanakOblasti { get; set; }
        public ICollection<ReporterClanak> ReporterClanak { get; set; }
        public Placanje Placanje { get; set; }

    }
}
