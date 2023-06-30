namespace NovinskaAgencija.data.model
{
    public class Clanak
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string? ImageUrl { get; set; } = string.Empty;
        public int Cena { get; set; }
        public string? FileUrl { get; set; } = string.Empty;
        public DateTime PublishDate { get; set; }
        public int OblastId { get; set; }
        public Oblast Oblast { get; set; }
        public int? ReporterId { get; set; }
        public Reporter? Reporter { get; set; }
        public Placanje? Placanje { get; set; }

    }
}
