using Microsoft.AspNetCore.Mvc;
using NovinskaAgencija.data.DTO.Article.request;

namespace NovinskaAgencija.services.ArticleService
{
    public interface IArticleService
    {
        public IActionResult AddArticle(AddArticleRequest request);
        //public IActionResult UploadFile(AddArticleRequest request);
    }
}
