using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using MimeKit.Text;
using Newtonsoft.Json.Linq;
using NovinskaAgencija.data.DTO;
using NovinskaAgencija.data.DTO.Article.response;
using NovinskaAgencija.data.DTO.Auth.request;
using NovinskaAgencija.data.DTO.Auth.response;
using NovinskaAgencija.data.model;
using NovinskaAgencija.services.JWTService;
using Org.BouncyCastle.Crypto;
using System.Security.Cryptography;

namespace NovinskaAgencija.services.AuthService
{
    public class AuthService : IAuthService
    {
        public DataContext context { get; set; }
        private readonly IConfiguration _configuration;
        private readonly IJwtService _jwtService;
        public AuthService(DataContext context, IConfiguration configuration, IJwtService jwtService)
        { 
            this.context = context;
            _configuration = configuration;
            _jwtService = jwtService;
        }
        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public void SendEmail(string email, string body)
        {
            var sender = _configuration.GetValue<string>("Mail:email");
            var password = _configuration.GetValue<string>("Mail:password");
            var emailSender = new MimeMessage();
            emailSender.From.Add(new MailboxAddress("Novinska Agencija", sender));
            emailSender.To.Add(MailboxAddress.Parse(email));
            emailSender.Subject = "Novinska Agencija";
            emailSender.Body = new TextPart(TextFormat.Html)
            {
                Text=body
            };
            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, false);
            smtp.Authenticate(sender, password);
            smtp.Send(emailSender);
            smtp.Disconnect(true);
        }

        public IActionResult RegisterReporter(RegisterReporterRequest request)
        {
            if(context.Users.Any(x => x.Username == request.Username || x.Email == request.Email))
            {
                return new BadRequestObjectResult(new {error = "User already exists"});
            }

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            User user = new User();
            user.Username = request.Username;
            user.Email = request.Email;
            user.PasswordHash = passwordHash;
            user.PassswordSalt = passwordSalt;
            user.StateOfOrigin = request.StateOfOrigin;
            user.JurassicAccount = request.JurassicAccount;
            user.Role = Role.Reporter;

            Random random = new Random();
            int randomNumber = random.Next(100000, 999999);

            user.VerificationToken = randomNumber.ToString();

            context.Users.Add(user);
            context.SaveChanges();

            Reporter reporter = new Reporter();
            reporter.Ime = request.Ime;
            reporter.Prezime = request.Prezime;
            reporter.User = user;

            context.Reporteri.Add(reporter);
            context.SaveChanges();
            SendEmail(request.Email, "Your Verification Token is: " + randomNumber.ToString());

            return new OkObjectResult(new {message = "User created" });
        }

        public IActionResult RegisterKlijent(RegisterKlijentRequest request)
        {
            if (context.Users.Any(x => x.Username == request.Username || x.Email == request.Email))
            {
                return new BadRequestObjectResult(new {error = "Username already exists" });
            }

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            User user = new User();
            user.Username = request.Username;
            user.Email = request.Email;
            user.PasswordHash = passwordHash;
            user.PassswordSalt = passwordSalt;
            user.StateOfOrigin = request.StateOfOrigin;
            user.JurassicAccount = request.JurassicAccount;
            user.Role = Role.Client;

            Random random = new Random();
            int randomNumber = random.Next(100000, 999999);

            user.VerificationToken = randomNumber.ToString();

            context.Users.Add(user);
            context.SaveChanges();

            Klijent klijent = new Klijent();
            klijent.NazivKompanije = request.NazivKompanije;
            klijent.User = user;
            klijent.TipPreduzeca = request.TipPreduzeca;


            context.Klijenti.Add(klijent);
            context.SaveChanges();
            SendEmail(request.Email, "Your Verification Token is: " + randomNumber.ToString());

            return new OkObjectResult(new {message = "User created" });
        }

        public IActionResult Login(LoginRequest request)
        {
                User user = context.Users.Where(u => u.Email == request.Email).FirstOrDefault();
                bool verified = user.VerificationToken != null ? false : true;
                if (user == null)
                {
                    return new BadRequestObjectResult(new {error = "User with this email doesn't exist" });
                }
                if(!VerifyPasswordHash(request.Password, user.PasswordHash, user.PassswordSalt))
                {
                    return new BadRequestObjectResult(new {error = "Wrong password" });
                }
                if(user.VerificationToken != null)
                {
                return new OkObjectResult(new LoginReporterResponse
                {
                    Username = user.Username,
                    Email = user.Email,
                    Role = user.Role,
                    StateOfOrigin = user.StateOfOrigin,
                    JurassicAccount = user.JurassicAccount,
                    IsVerified = verified
                });
                }
                string token = _jwtService.CreateToken(user);
            var placanje = context.Placanja.Where(p => p.User == user).ToList();
            if (user.Role == Role.Reporter)
                {
                    Reporter reporter = context.Reporteri.Where(r => r.User == user).FirstOrDefault();
                //var clanci = context.ReporterClanak.Where(rc => rc.Reporter == reporter).ToList();
                var article = context.Clanci.Include(c => c.Oblast).Include(c => c.Reporter).Select(c => new
                {
                    c.Oblast.Name,
                    c.Title,
                    c.Content,
                    c.Cena,
                    c.PublishDate,
                    c.ImageUrl,
                    c.FileUrl,
                    c.Reporter.Id,
                    c.Reporter.Ime,
                    c.Reporter.Prezime,
                }).ToList();

                List<ArticleResponse> articles = new List<ArticleResponse>();

                if (article != null)
                {
                    foreach (var item in article)
                    {
                        articles.Add(new ArticleResponse
                        {
                            OblastName = item.Name,
                            Title = item.Title,
                            Content = item.Content,
                            Cena = item.Cena,
                            PublishDate = item.PublishDate,
                            ImageUrl = item.ImageUrl,
                            FileUrl = item.FileUrl,
                            ReporterId = item.Id,
                            ReporterIme = item.Ime,
                            ReporterPrezime = item.Prezime
                        });
                    }
                }
                else
                {
                    articles = null;
                }
                return new OkObjectResult(new LoginReporterResponse
                    {
                        Username = user.Username,
                        Email = user.Email,
                        Token = token,
                        Role = user.Role,
                        StateOfOrigin = user.StateOfOrigin,
                        JurassicAccount = user.JurassicAccount,
                        Clanci = articles,
                        Ime = reporter.Ime,
                        Prezime = reporter.Prezime,
                        IsVerified = verified,
                        Placanja = placanje
                    });
                }
                if(user.Role == Role.Client)
                {
                    Klijent klijent = context.Klijenti.Where(k => k.User == user).FirstOrDefault();
                    return new OkObjectResult(new LoginKlijentResponse
                    {
                        Username = user.Username,
                        Email = user.Email,
                        Token = token,
                        Role = user.Role,
                        Placanje = placanje,
                        StateOfOrigin = user.StateOfOrigin,
                        JurassicAccount = user.JurassicAccount,
                        NazivKompanije = klijent.NazivKompanije,
                        TipPreduzeca = klijent.TipPreduzeca,
                        IsVerified = verified
                    });
                }
                return new BadRequestObjectResult(new {error = "Wrong role" });
        }
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using(var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        public IActionResult Verify(VerifyRequest request)
        {
            User user = context.Users.Where(u => u.Email == request.Email).FirstOrDefault();
            if (user == null)
            {
                return new BadRequestObjectResult(new {error = "User with this email doesn't exist" });
            }
            if(user.VerificationToken != request.Token)
            {
                return new BadRequestObjectResult(new {error = "Wrong verification token" });
            }
            user.VerificationToken = null;
            context.SaveChanges();

            string token = _jwtService.CreateToken(user);
            var placanje = context.Placanja.Where(p => p.User == user).ToList();
            if (user.Role == Role.Reporter)
            {
                Reporter reporter = context.Reporteri.Where(r => r.User == user).FirstOrDefault();
                var article = context.Clanci.Include(c => c.Oblast).Include(c => c.Reporter).Select(c => new
                {
                    c.Oblast.Name,
                    c.Title,
                    c.Content,
                    c.Cena,
                    c.PublishDate,
                    c.ImageUrl,
                    c.FileUrl,
                    c.Reporter.Id,
                    c.Reporter.Ime,
                    c.Reporter.Prezime,
                }).ToList();

                List<ArticleResponse> articles = new List<ArticleResponse>();

                if (article != null)
                {
                    foreach (var item in article)
                    {
                        articles.Add(new ArticleResponse
                        {
                            OblastName = item.Name,
                            Title = item.Title,
                            Content = item.Content,
                            Cena = item.Cena,
                            PublishDate = item.PublishDate,
                            ImageUrl = item.ImageUrl,
                            FileUrl = item.FileUrl,
                            ReporterId = item.Id,
                            ReporterIme = item.Ime,
                            ReporterPrezime = item.Prezime
                        });
                    }
                }
                else
                {
                    articles = null;
                }

                return new OkObjectResult(new LoginReporterResponse
                {
                    Username = user.Username,
                    Email = user.Email,
                    Token = token,
                    Role = user.Role,
                    StateOfOrigin = user.StateOfOrigin,
                    JurassicAccount = user.JurassicAccount,
                    Clanci = articles,
                    Ime = reporter.Ime,
                    Prezime = reporter.Prezime,
                    IsVerified = true,
                    Placanja = placanje
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
                    Placanje = placanje,
                    StateOfOrigin = user.StateOfOrigin,
                    JurassicAccount = user.JurassicAccount,
                    NazivKompanije = klijent.NazivKompanije,
                    TipPreduzeca = klijent.TipPreduzeca,
                    IsVerified = true
                });
            }
        }

        public IActionResult ForgotPassword(ForgotPasswordRequest request)
        {
            User user = context.Users.Where(u => u.Email == request.Email).FirstOrDefault();
            if (user == null)
            {
                return new BadRequestObjectResult(new { error = "User with this email doesn't exist" });
            }
            Random random = new Random();
            int randomNumber = random.Next(100000, 999999);
            user.ForgotPasswordToken = randomNumber.ToString();

            context.SaveChanges();

            SendEmail(request.Email, "Your ForgotPassword Token is: " + randomNumber.ToString());

            return new OkObjectResult(new { message = "ForgotPassword token sent" });
        }

        public IActionResult ResetPassword(ResetPasswordRequest request)
        {
            User user = context.Users.Where(u => u.Email == request.Email).FirstOrDefault();
            if (user == null)
            {
                return new BadRequestObjectResult(new { error = "User with this email doesn't exist" });
            }
            if(user.ForgotPasswordToken != request.Token)
            {
                return new BadRequestObjectResult(new { error = "Wrong ForgotPassword token" });
            }
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PassswordSalt = passwordSalt;
            user.ForgotPasswordToken = null;
            context.SaveChanges();
            return new OkObjectResult(new { message = "Password changed" });
        }

        public IActionResult ResendVerificationEmail(ForgotPasswordRequest request)
        {
            User user = context.Users.Where(u => u.Email == request.Email).FirstOrDefault();
            if (user == null)
            {
                return new BadRequestObjectResult(new { error = "User with this email doesn't exist" });
            }
            if(user.VerificationToken == null)
            {
                return new BadRequestObjectResult(new { error = "User is already verified" });
            }
            SendEmail(request.Email, "Your Verification Token is: " + user.VerificationToken);
            return new OkObjectResult(new { message = "Verification token sent" });
        }

        
    }
}
