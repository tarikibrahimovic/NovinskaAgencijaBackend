using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MimeKit.Text;
using MimeKit;
using NovinskaAgencija.data.model;
using NovinskaAgencija.services.AuthService;
using MailKit.Net.Smtp;
using NovinskaAgencija.data.DTO.Auth.request;

namespace NovinskaAgencija.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public IAuthService service { get; set; }
        private readonly IConfiguration _configuration;

        public AuthController(IAuthService service, IConfiguration configuration)
        {
            this.service = service;
            _configuration = configuration;
        }

        [HttpPost("registerReporter")]
        public IActionResult RegisterReporter([FromBody] RegisterReporterRequest request)
        {
            try
            {
                var result = service.RegisterReporter(request);
                return result;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("registerKlijent")]
        public IActionResult RegisterKlijent([FromBody] RegisterKlijentRequest request)
        {
            try
            {
                var result = service.RegisterKlijent(request);
                return result;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            try
            {
                var result = service.Login(request);
                return result;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("verify")]
        public IActionResult Verify([FromBody] VerifyRequest request)
        {
            try
            {
                var result = service.Verify(request);
                return result;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("forgotPassword")]
        public IActionResult ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            try
            {
                var result = service.ForgotPassword(request);
                return result;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("resetPassword")]
        public IActionResult ResetPassword([FromBody] ResetPasswordRequest request)
        {
            try
            {
                var result = service.ResetPassword(request);
                return result;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("resendVerificationEmail")]
        public IActionResult ResendVerificationEmail([FromBody] ForgotPasswordRequest request)
        {
            try
            {
                var result = service.ResendVerificationEmail(request);
                return result;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
