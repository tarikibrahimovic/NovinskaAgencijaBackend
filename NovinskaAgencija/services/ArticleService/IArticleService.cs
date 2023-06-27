using Microsoft.AspNetCore.Mvc;
using NovinskaAgencija.data.DTO.Article.request;
using NovinskaAgencija.data.model;

namespace NovinskaAgencija.services.ArticleService
{
    public interface IArticleService
    {
        public IActionResult AddArticle(AddArticleRequest request);
        //public IActionResult UploadFile(AddArticleRequest request);
        public IActionResult GetArticles();
        public IActionResult DeleteArticle(int articleId, int userId);
        public IActionResult BuyArticle(BuyArticleRequest request);
        public IActionResult GetReporters();
        public IActionResult GetArticle(int articleId);
    }
}
