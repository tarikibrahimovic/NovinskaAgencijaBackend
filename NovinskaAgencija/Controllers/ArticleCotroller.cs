using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NovinskaAgencija.data.DTO.Article.request;
using NovinskaAgencija.services.ArticleService;

namespace NovinskaAgencija.Controllers
{
    [Route("api/article")]
    [ApiController]
    [Authorize]
    public class ArticleCotroller : ControllerBase
    {
        public IArticleService service{ get; set; }
        public ArticleCotroller(IArticleService service)
        {
            this.service = service;
        }

        [HttpPost("addArticle")]
        public IActionResult AddArticle([FromForm] AddArticleRequest request)
        {
            try
            {
                var response = service.AddArticle(request);
                return response;
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Greska na serveru");
            }
        }

        [AllowAnonymous]
        [HttpGet("articles")]
        public IActionResult GetArticles()
        {
            try
            {
                var response = service.GetArticles();
                return response;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska na serveru");
            }
        }

        [HttpDelete("deleteArticle/{articleId}")]
        public IActionResult DeleteArticle(int articleId)
        {
            try
            {
                var response = service.DeleteArticle(articleId);
                return response;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska na serveru");
            }
        }

        [HttpPost("buyArticle")]
        public IActionResult BuyArticle([FromBody] BuyArticleRequest request)
        {
            try
            {
                var response = service.BuyArticle(request);
                return response;
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Greska na serveru");
            }
        }

        [AllowAnonymous]
        [HttpGet("reporters")]
        public IActionResult GetReporters()
        {
            try
            {
                var response = service.GetReporters();
                return response;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska na serveru");
            }
        }

        [AllowAnonymous]
        [HttpGet("getArticle/{articleId}")]
        public IActionResult GetArticle(int articleId)
        {
            try
            {
                var response = service.GetArticle(articleId);
                return response;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska na serveru");
            }
        }

        [HttpGet("usersArticles")]
        public IActionResult GetUsersArticles()
        {
            try
            {
                var response = service.GetUsersArticles();
                return response;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska na serveru");
            }
        }

        [HttpGet("boughtArticles")]
        public IActionResult GetBoughtArticles()
        {
            try
            {
                var response = service.GetBoughtArticles();
                return response;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska na serveru");
            }
        }

        [HttpGet("personalArticles/{articleId}")]
        public IActionResult GetPersonalArticles(int articleId)
        {
            try
            {
                var response = service.GetPersonalArticles(articleId);
                return response;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska na serveru");
            }
        }
    }
}
