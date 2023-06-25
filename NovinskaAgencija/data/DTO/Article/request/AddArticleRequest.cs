namespace NovinskaAgencija.data.DTO.Article.request
{
    public class AddArticleRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int Price { get; set; }
        public int CategoryId { get; set; }
        public IFormFile? Image { get; set; }
        public IFormFile? File { get; set; }
    }
}
