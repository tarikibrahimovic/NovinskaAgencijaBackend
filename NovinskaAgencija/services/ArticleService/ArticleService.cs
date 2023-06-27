using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NovinskaAgencija.data.DTO.Article.request;
using NovinskaAgencija.data.model;
using NovinskaAgencija.services.AuthService;
using NovinskaAgencija.services.JWTService;
using System.Security.Claims;

namespace NovinskaAgencija.services.ArticleService
{
    public class ArticleService : IArticleService
    {
        public IHttpContextAccessor _acc { get; set; }
        public DataContext context { get; set; }
        private IConfiguration _configuration;
        private Cloudinary cloudinary;
        private Account account;
        private IJwtService jwtService;
        private IAuthService authService;
        public ArticleService(IHttpContextAccessor acc, DataContext context, IConfiguration configuration, IJwtService jwtService, IAuthService authService)
        {
            _acc = acc;
            this.context = context;
            _configuration = configuration;
            account = new Account(configuration.GetSection("Cloudinary:Cloud").Value,
                configuration.GetSection("Cloudinary:ApiKey").Value,
                configuration.GetSection("Cloudinary:ApiSecret").Value);
            cloudinary = new Cloudinary(account);
            this.jwtService = jwtService;
            this.authService = authService;
        }
        public IActionResult AddArticle(AddArticleRequest request)
        {
            var userId = int.Parse(_acc.HttpContext.User.FindFirstValue(ClaimTypes.PrimarySid));
            User user = context.Users.FirstOrDefault(u => u.Id == userId);
            Reporter reporter = context.Reporteri.FirstOrDefault(r => r.UserId == userId);
            Oblast oblast = context.Oblasti.FirstOrDefault(o => o.Id == int.Parse(request.CategoryId));

            if (reporter == null)
            {
                return new BadRequestObjectResult(new { error = "Niste reporter" });
            }

            if (oblast == null)
            {
                return new BadRequestObjectResult(new { error = "Oblast ne postoji" });
            }

            var imageLink = "";
            var fileUrl = "";

            if(request.Image != null)
            {
                var filePath = Path.GetTempFileName();

                using (var streamImage = System.IO.File.Create(filePath))
                {
                    request.Image.CopyTo(streamImage);
                }
                var profilePicturePublicId = $"{_configuration.GetSection("Cloudinary:ProfilePicsFolderName").Value}/user{userId}_profile-picture";
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(filePath),
                    PublicId = profilePicturePublicId,
                };
                var uploadResultImage = cloudinary.Upload(uploadParams);
                imageLink = uploadResultImage.Url.ToString();

            }
            if(request.File != null)
            {
                fileUrl = UploadFile(request);
            }


            Clanak clanak = new Clanak
            {
                Title = request.Title,
                Content = request.Content,
                Cena = int.Parse(request.Price),
                PublishDate = DateTime.Now,
                ImageUrl = imageLink,
                Reporter = reporter,
                Oblast = oblast,
                FileUrl = fileUrl,
            };
            context.Clanci.Add(clanak);
            context.SaveChanges();

            return new OkObjectResult(clanak);
        }

        public string UploadFile(AddArticleRequest request)
        {
            try
            {
                var fileName = Path.GetFileNameWithoutExtension(request.File.FileName);
                var fileExtension = Path.GetExtension(request.File.FileName);
                var tempPath = Path.Combine(Path.GetTempPath(), $"{fileName}{fileExtension}");
                using (var stream = new FileStream(tempPath, FileMode.Create))
                {
                    request.File.CopyTo(stream);
                }
                var guid = Guid.NewGuid().ToString();
                var profilePicturePublicId = $"{_configuration.GetSection("Cloudinary:ProfilePicsFolderName").Value}/{guid}/clanak-file/{fileName}{fileExtension}";
                var uploadParams = new RawUploadParams()
                {
                    File = new FileDescription(tempPath),
                    PublicId = profilePicturePublicId,
                };
                var uploadResult = cloudinary.Upload(uploadParams);
                return uploadResult.Url.ToString();
            }
            catch (Exception ex)
            {

                return ex.Message;
            }
        }

        public IActionResult GetArticles()
        {

            var articles = context.Clanci.Include(c => c.Oblast).Include(c => c.Reporter).Where(c => c.Placanje == null).Select(c => new
            {
                c.Id,
                c.Oblast.Name,
                c.Title,
                c.Content,
                c.Cena,
                c.PublishDate,
                c.ImageUrl,
                c.FileUrl,
                c.ReporterId,
                c.Reporter.Ime,
                c.Reporter.Prezime,
            });
            return new OkObjectResult(articles);
        }

        public IActionResult DeleteArticle(int articleId, int reporterId)
        {
            int userId = int.Parse(_acc.HttpContext.User.FindFirstValue(ClaimTypes.PrimarySid));
            User user = context.Users.FirstOrDefault(u => u.Id == userId);
            Reporter reporter = context.Reporteri.FirstOrDefault(r => r.UserId == userId);
            if (reporter.Id != reporterId)
            {
                return new BadRequestObjectResult(new { error = "Niste autor ovog clanka" });
            }
            Clanak clanak = context.Clanci.FirstOrDefault(c => c.Id == articleId);
            if (reporter == null || user == null)
            {
                return new BadRequestObjectResult(new { error = "Niste reporter" });
            }
            if (clanak == null)
            {
                return new BadRequestObjectResult(new { error = "Clanak ne postoji" });
            }
            if (clanak.Reporter.Id != reporter.Id)
            {
                return new BadRequestObjectResult(new { error = "Niste autor ovog clanka" });
            }

            Placanje placanje = context.Placanja.FirstOrDefault(p => p.ClanakId == clanak.Id);
            if (placanje != null)
            {
                return new BadRequestObjectResult(new { error = "Clanak je kupljen" });
            }

            context.Clanci.Remove(clanak);
            context.SaveChanges();
            return new OkObjectResult(new { message = "Clanak uspesno obrisan" });
        }

        public IActionResult BuyArticle(BuyArticleRequest request)
        {
            int userId = int.Parse(_acc.HttpContext.User.FindFirstValue(ClaimTypes.PrimarySid));
            User user = context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return new BadRequestObjectResult(new { error = "Niste korisnik" });
            }
            if(user.JurassicAccount != request.JurassicAccount)
            {
                return new BadRequestObjectResult(new { error = "Pogresan bankovni racun" });
            }
            Clanak clanak = context.Clanci.FirstOrDefault(c => c.Id == request.ArticleId);
            Reporter reporter = context.Reporteri.FirstOrDefault(r => r.Id == clanak.ReporterId);
            
            Placanje placanjeProvera = context.Placanja.FirstOrDefault(p => p.Clanak.Id == clanak.Id && p.User.Id == user.Id);
            if (placanjeProvera != null)
            {
                return new BadRequestObjectResult(new { error = "Vec ste kupili ovaj clanak" });
            }

            if (clanak == null)
            {
                return new BadRequestObjectResult(new { error = "Clanak ne postoji" });
            }
            if (clanak.Placanje != null)
            {
                return new BadRequestObjectResult(new { error = "Clanak je vec kupljen" });
            }
            Placanje placanje = new Placanje
            {
                User = user,
                Clanak = clanak,
                TransactionDate = DateTime.Now,
            };
            context.Placanja.Add(placanje);
            context.SaveChanges();
            authService.SendEmail(reporter.User.Email, "Your Article has been bought!");

            return new OkObjectResult(new { message = "Article successfully bought!" });
        }

        public IActionResult GetArticle(int articleId)
        {
            var articles = context.Clanci.Include(c => c.Oblast).Include(c => c.Reporter).Where(c => c.Placanje == null && c.Id == articleId).Select(c => new
            {
                c.Id,
                c.Oblast.Name,
                c.Title,
                c.Content,
                c.Cena,
                c.PublishDate,
                c.ImageUrl,
                c.FileUrl,
                c.ReporterId,
                c.Reporter.Ime,
                c.Reporter.Prezime,
            }).FirstOrDefault();
            return new OkObjectResult(articles);
        }

        public IActionResult GetReporters()
        {
            var reporters = context.Reporteri.Include(r => r.User).Include(r => r.Clanak).Where(r => r.Clanak.Count > 0).Select(r => new
            {
                r.Id,
                r.Ime,
                r.Prezime,
            });
            return new OkObjectResult(reporters);
        }
    }
}
