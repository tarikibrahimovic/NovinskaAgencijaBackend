using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NovinskaAgencija.data.DTO.User.request;
using NovinskaAgencija.data.model;
using System.Security.Claims;
using System.Security.Cryptography;
using static System.Net.Mime.MediaTypeNames;
using NovinskaAgencija.data.DTO.Auth.request;
using NovinskaAgencija.data.DTO.Auth.response;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Crypto;
using NovinskaAgencija.services.JWTService;

namespace NovinskaAgencija.services.UserService
{
    public class UserService : IUserService
    {
        public IHttpContextAccessor _acc { get; set; }
        public DataContext context { get; set; }
        private IConfiguration _configuration;
        private Cloudinary cloudinary;
        private Account account;
        private IJwtService jwtService;
        public UserService(IHttpContextAccessor acc, DataContext context, IConfiguration configuration, IJwtService jwtService)
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
        public IActionResult ChangeUsername(ChangeUsernameRequest request)
        {
            try
            {
                int id = int.Parse(_acc.HttpContext.User.FindFirstValue(ClaimTypes.PrimarySid));
                User user = context.Users.Where(u => u.Id == id).FirstOrDefault();
                if (user == null)
                {
                    return new BadRequestObjectResult(new { message = "User not found" });
                }
                user.Username = request.Username;
                context.SaveChanges();
                return new OkObjectResult(new { message = "Username changed" });
            }
            catch (Exception ex)
            {

                return new BadRequestObjectResult(new { message = ex.Message });
            }
            
        }

        public IActionResult ChangePassword(ChangePasswordRequest request)
        {
            try
            {
                int id = int.Parse(_acc.HttpContext.User.FindFirstValue(ClaimTypes.PrimarySid));
                User user = context.Users.Where(u => u.Id == id).FirstOrDefault();
                if (user == null)
                {
                    return new BadRequestObjectResult(new { message = "User not found" });
                }
                if (!VerifyPasswordHash(request.OldPassword, user.PasswordHash, user.PassswordSalt))
                {
                    return new BadRequestObjectResult("Wrong password");
                }

                CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
                user.PasswordHash = passwordHash;
                user.PassswordSalt = passwordSalt;
                context.SaveChanges();
                return new OkObjectResult(new { message = "Password changed" });
            }
            catch (Exception ex)
            {

                return new BadRequestObjectResult(new { message = ex.Message });
            }
            
        }

        public IActionResult ProfileImage(ImageRequest request)
        {
            try
            {
                if (request.ProfilePicture == null)
                {
                    return new BadRequestObjectResult(new { message = "No image" });
                }
                var userId = int.Parse(_acc.HttpContext.User.FindFirstValue(ClaimTypes.PrimarySid));
                User user = context.Users.FirstOrDefault(u => u.Id == userId);

                var filePath = Path.GetTempFileName();

                using (var stream = System.IO.File.Create(filePath))
                {
                    request.ProfilePicture.CopyTo(stream);
                }
                var profilePicturePublicId = $"{_configuration.GetSection("Cloudinary:ProfilePicsFolderName").Value}/user{userId}_profile-picture";
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(filePath),
                    PublicId = profilePicturePublicId,
                };
                var uploadResult = cloudinary.Upload(uploadParams);
                user.ImageUrl = uploadResult.Url.ToString();
                context.SaveChanges();
                return new OkObjectResult(new { pictureUrl = uploadResult.Url });
            }
            catch (Exception ex)
            {

                return new BadRequestObjectResult(new { message = ex.Message });
            }
        }

        public IActionResult DeleteProfileImage()
        {
            try
            {
                var userId = int.Parse(_acc.HttpContext.User.FindFirstValue(ClaimTypes.PrimarySid));
                User user = context.Users.FirstOrDefault(u => u.Id == userId);
                if (user.ImageUrl != null)
                {
                    user.ImageUrl = null;
                    var profilePicturePublicId = $"{_configuration.GetSection("Cloudinary:ProfilePicsFolderName").Value}/user{userId}_profile-picture";
                    var deletionParams = new DeletionParams(profilePicturePublicId)
                    {
                        ResourceType = ResourceType.Image
                    };

                    cloudinary.Destroy(deletionParams);
                    context.SaveChanges();
                    return new OkObjectResult(new { message = "Picture delete succesfully" });
                }
                else
                {
                    return new BadRequestObjectResult(new { message = "You don't have a picture" });
                }
            }
            catch (Exception ex)
            {

                return new BadRequestObjectResult(new { message = ex.Message });
            }
        }

        public IActionResult DeleteProfile(DeleteProfileRequest request)
        {
            try
            {
                int userId = int.Parse(_acc.HttpContext.User.FindFirstValue(ClaimTypes.PrimarySid));
                User user = context.Users.FirstOrDefault(u => u.Id == userId);
                if (user == null)
                {
                    return new BadRequestObjectResult(new { message = "User not found" });
                }
                if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PassswordSalt))
                {
                    return new BadRequestObjectResult("Wrong password");
                }
                context.Users.Remove(user);
                context.SaveChanges();
                return new OkObjectResult(new { message = "Profile deleted" });
            }
            catch (Exception)
            {

                return new BadRequestObjectResult(new { message = "Something went wrong" });
            }

        }

        public IActionResult RefreshToken()
        {
            int userId = int.Parse(_acc.HttpContext.User.FindFirstValue(ClaimTypes.PrimarySid));
            User user = context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return new BadRequestObjectResult(new { error = "User not found" });
            }
            var token = jwtService.CreateToken(user);
            List<Placanje> placanja = user.Placanja.ToList();
            if(user.Role == data.model.Role.Reporter)
            {
                Reporter reporter = context.Reporteri.Where(r => r.User == user).FirstOrDefault();
                //var clanci = context.ReporterClanak.Where(rc => rc.Reporter == reporter).ToList();
                var clanci = context.Clanci.Where(c => c.ReporterId == reporter.Id).ToList();
                return new OkObjectResult(new LoginReporterResponse
                {
                    Username = user.Username,
                    Email = user.Email,
                    Token = token,
                    Role = user.Role,
                    StateOfOrigin = user.StateOfOrigin,
                    JurassicAccount = user.JurassicAccount,
                    ReporterClanci = clanci,
                    Ime = reporter.Ime,
                    Prezime = reporter.Prezime,
                    IsVerified = user.VerificationToken == null ? true : false,
                    Placanja = placanja,
                });
            }
            else
            {
                Klijent klijent = context.Klijenti.Where(k => k.User == user).FirstOrDefault();
                return new OkObjectResult(new LoginKlijentResponse
                {
                    Username = user.Username,
                    Email = user.Email,
                    Token = token,
                    Role = user.Role,
                    Placanje = placanja,
                    StateOfOrigin = user.StateOfOrigin,
                    JurassicAccount = user.JurassicAccount,
                    NazivKompanije = klijent.NazivKompanije,
                    TipPreduzeca = klijent.TipPreduzeca,
                    IsVerified = user.VerificationToken == null ? true : false
                });
            }

        }

        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}
