namespace NovinskaAgencija.data.DTO.Article.request
{
    public class AddArticleRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Price { get; set; } = string.Empty;
        public string CategoryId { get; set; } = string.Empty;
        public IFormFile? Image { get; set; }
        public IFormFile? File { get; set; }
    }
}
