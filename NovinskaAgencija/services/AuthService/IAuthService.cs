using Microsoft.AspNetCore.Mvc;
using NovinskaAgencija.data.DTO.Auth.request;

namespace NovinskaAgencija.services.AuthService
{
    public interface IAuthService
    {
        public IActionResult RegisterReporter(RegisterReporterRequest request);
        public IActionResult RegisterKlijent(RegisterKlijentRequest request);
        public IActionResult Login(LoginRequest request);
        public IActionResult Verify(VerifyRequest request);
        public IActionResult ForgotPassword(ForgotPasswordRequest request);
        public IActionResult ResetPassword(ResetPasswordRequest request);
        public IActionResult ResendVerificationEmail(ForgotPasswordRequest request);
    }
}
