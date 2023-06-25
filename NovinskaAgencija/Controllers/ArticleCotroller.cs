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

        //[HttpPost("addFile")]
        //public IActionResult AddFile([FromForm] AddArticleRequest request)
        //{
        //    try
        //    {
        //        var response = service.UploadFile(request);
        //        return response;
        //    }
        //    catch (Exception)
        //    {

        //        return StatusCode(StatusCodes.Status500InternalServerError, "Greska na serveru");
        //    }
        //}

    }
}
