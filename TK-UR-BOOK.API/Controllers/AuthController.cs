using Microsoft.AspNetCore.Mvc;
using TK_UR_BOOK.Application.Interfaces;
using TK_UR_BOOK.Application.UseCases.UserCommands;
using TK_UR_BOOK.Domain.Entities;

namespace TK_UR_BOOK.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("Login")]
        public IActionResult Login(LogInCommand requst)
        {
            var result = _authService.LogIn(requst);
            if (result.Result.IsFailure)
            {
                return Unauthorized(result.Result.Error);
            }


            return Ok(result);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterUserCommand requst)
        {
            var result = await _authService.RegisterNewUser(requst);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }
            return Ok(new { Message = "User Registered Successfully", Id = result.Value });
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken(string accesToken, string refreshToken)
        {
            var result = await _authService.RefreshAccesToken(accesToken, refreshToken);
            if (result.IsFailure)
            {
                return Unauthorized(result.Error);
            }
            return Ok(result.Value);
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout(string token)
        {
            var result = await _authService.LogOut(token);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }
            return Ok(new { Message = "User Logged Out Successfully" });

        }


        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(string email, string currentPassword, string newPassword)
        {
            var result = await _authService.ChangePassword(email, currentPassword, newPassword);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }
            return Ok(new { Message = "Password Changed Successfully" });

        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var result = await _authService.ForgotPassword(email);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }
            return Ok(new { Message = "Password Reset Email Sent Successfully" });

        }
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(string email, string token, string newPassword)
        {
            var result = await _authService.ResetPassword(email, token, newPassword);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }
            return Ok(new { Message = "Password Reset Successfully" });
        }
        [HttpPost("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailCommand requst)
        {
            var result = await _authService.ConfirmEmail(requst);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }
            return Ok(new { Message = "Email Confirmed Successfully" });
        }

        [HttpPost("ResendConfirmationEmail")]
        public async Task<IActionResult> ResendConfirmationEmail(string email)
        {
            var result = await _authService.ResendConfirmationEmail(email);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }
            return Ok(new { Message = "Confirmation Email Resent Successfully" });


        }
        [HttpPost("NewRefreshToken")]
        public async Task<IActionResult> NewRefreshToken(RefreshToken Token)
        {
            var result = await _authService.UpdateRefreshToken(Token);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }
            return Ok(new { Message = "Refresh Token Updated Successfully" });
        }
    }
}

