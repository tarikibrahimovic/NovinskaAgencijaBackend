namespace NovinskaAgencija.data.DTO.Article.response
{
    public class ArticleResponse
    {
        public int Id { get; set; }
        public string OblastName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int Cena { get; set; }
        public DateTime PublishDate { get; set; }
        public string? ImageUrl { get; set; } = string.Empty;
        public string? FileUrl { get; set; } = string.Empty;
        public int ReporterId { get; set; }
        public string ReporterIme { get; set; } = string.Empty;
        public string ReporterPrezime { get; set; } = string.Empty;

    }
}
