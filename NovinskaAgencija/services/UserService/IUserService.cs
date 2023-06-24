using Microsoft.AspNetCore.Mvc;
using NovinskaAgencija.data.DTO.User.request;

namespace NovinskaAgencija.services.UserService
{
    public interface IUserService
    {
        public IActionResult ChangeUsername(ChangeUsernameRequest request);
        public IActionResult ChangePassword(ChangePasswordRequest request);
        public IActionResult ProfileImage(ImageRequest request);
        public IActionResult DeleteProfileImage();
        public IActionResult DeleteProfile(DeleteProfileRequest request);
        public IActionResult RefreshToken();
    }
}
