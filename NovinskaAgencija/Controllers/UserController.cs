using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NovinskaAgencija.data.DTO.Auth.request;
using NovinskaAgencija.data.DTO.User.request;
using NovinskaAgencija.services.UserService;

namespace NovinskaAgencija.Controllers
{
    [Route("api/user")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        public IUserService service { get; set; }
        public UserController(IUserService service)
        {
            this.service = service;
        }

        [HttpPost("changeUsername")]
        public IActionResult ChangeUsername([FromBody] ChangeUsernameRequest request)
        {
            try
            {
                var result = service.ChangeUsername(request);
                return result;
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Greska na serveru");
            }
        }

        [HttpPost("changePassword")]
        public IActionResult ChangePassword([FromBody] ChangePasswordRequest request)
        {
            try
            {
                var result = service.ChangePassword(request);
                return result;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska na serveru");
            }
        }

        [HttpPost("profileImage")]
        public IActionResult ProfileImage([FromForm] ImageRequest request)
        {
            try
            {
                var result = service.ProfileImage(request);
                return result;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska na serveru");
            }
        }

        [HttpDelete("deleteProfileImage")]
        public IActionResult DeleteProfileImage()
        {
            try
            {
                var result = service.DeleteProfileImage();
                return result;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska na serveru");
            }
        }

        [HttpPost("deleteProfile")]
        public IActionResult DeleteProfile([FromBody] DeleteProfileRequest request)
        {
            try
            {
                var result = service.DeleteProfile(request);
                return result;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska na serveru");
            }
        }

        [HttpGet("refreshToken")]
        public IActionResult RefreshToken()
        {
            try
            {
                var result = service.RefreshToken();
                return result;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
