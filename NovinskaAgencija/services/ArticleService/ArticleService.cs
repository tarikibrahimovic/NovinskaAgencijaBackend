using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using NovinskaAgencija.data.DTO.Article.request;
using NovinskaAgencija.data.model;
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
        public ArticleService(IHttpContextAccessor acc, DataContext context, IConfiguration configuration, IJwtService jwtService)
        {
            _acc = acc;
            this.context = context;
            _configuration = configuration;
            account = new Account(configuration.GetSection("Cloudinary:Cloud").Value,
                configuration.GetSection("Cloudinary:ApiKey").Value,
                configuration.GetSection("Cloudinary:ApiSecret").Value);
            cloudinary = new Cloudinary(account);
            this.jwtService = jwtService;

        }
        public IActionResult AddArticle(AddArticleRequest request)
        {
            var userId = int.Parse(_acc.HttpContext.User.FindFirstValue(ClaimTypes.PrimarySid));
            User user = context.Users.FirstOrDefault(u => u.Id == userId);
            Reporter reporter = context.Reporteri.FirstOrDefault(r => r.UserId == userId);
            Oblast oblast = context.Oblasti.FirstOrDefault(o => o.Id == request.CategoryId);

            if(reporter == null)
            {
                return new BadRequestObjectResult(new { error = "Niste reporter" });
            }

            if (oblast == null)
            {
                return new BadRequestObjectResult(new { error = "Oblast ne postoji" });
            }

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

            string fileUrl = UploadFile(request);

            Clanak clanak = new Clanak
            {
                Title = request.Title,
                Content = request.Content,
                Cena = request.Price,
                PublishDate = DateTime.Now,
                ImageUrl = uploadResultImage.Url.ToString(),
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
            var filePath = Path.GetTempFileName();
            using (var stream = System.IO.File.Create(filePath))
            {
                request.File.CopyTo(stream);
            }
            // Get the original file name without the extension
            var fileName = Path.GetFileNameWithoutExtension(request.File.FileName);

            // Get the file extension
            var fileExtension = Path.GetExtension(request.File.FileName);

            var profilePicturePublicId = $"{_configuration.GetSection("Cloudinary:ProfilePicsFolderName").Value}/clanak-file/{fileName}{fileExtension}";
            var uploadParams = new RawUploadParams()
            {
                File = new FileDescription(filePath),
                PublicId = profilePicturePublicId,
            };
            var uploadResult = cloudinary.Upload(uploadParams);
            //return new OkObjectResult(uploadResult.Url.ToString());
            return uploadResult.Url.ToString();
        }
    }
}
