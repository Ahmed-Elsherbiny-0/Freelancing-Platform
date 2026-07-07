using Api.Abstractions;
using Application.Dtos.Auth;
using Application.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> login([FromBody]LoginRequestDto request,CancellationToken cancellationToken)
        {
            var res = await authService.GetTokenAsync(request.Email, request.Password, cancellationToken);
            return res.IsSuccess ? Ok(res.Value) : res.ToProblem();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterRequestDto request, CancellationToken cancellationToken)
        {
            var res = await authService.Register(request, cancellationToken);
            return res.IsSuccess ? Ok() : res.ToProblem();
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshTokenRequestDto request,CancellationToken cancellationToken)
        {

             var   accessToken =  Request.Cookies["access_token"] ??string.Empty;
            var authResult = await authService.GetRefreshTokenAsync(accessToken, request.RefreshToken, cancellationToken);

            return authResult.IsSuccess ? Ok(authResult.Value) : authResult.ToProblem();
        }
        [HttpPost("revoke-refresh-token")]
        public async Task<IActionResult> RevokeRefreshToken(RefreshTokenRequestDto request,  CancellationToken cancellationToken)
        {
            var accessToken = Request.Cookies["access_token"] ?? string.Empty;

            var result = await authService.RevokeRefreshTokenAsync(accessToken, request.RefreshToken, cancellationToken);
            Response.Cookies.Delete("access_token");

            return result.IsSuccess ? Ok() : result.ToProblem();
        }
        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmationEmailRequestDto request, CancellationToken cancellationToken)
        {
            var result = await authService.ConfirmEmailAsync(request);

            return result.IsSuccess ? Ok() : result.ToProblem();
        }
        [HttpPost("resend-confirmation-email")]
        public async Task<IActionResult> ResendConfirmationEmail([FromBody] ResendConfirmationEmailRequestDto request, CancellationToken cancellationToken)
        {
            var result = await authService.ResendConfirmationEmailAsync(request);

            return result.IsSuccess ? Ok() : result.ToProblem();
        }

        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordRequestDto request)
        {
            var result = await authService.SendResetPasswordCodeAsync(request.Email);

            return result.IsSuccess ? Ok() : result.ToProblem();
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto request)
        {
            var result = await authService.ResetPasswordAsync(request);

            return result.IsSuccess ? Ok() : result.ToProblem();
        }

        [HttpGet("me")]
        public async Task<IActionResult> IsAuthinticated()
        {
            var accessToken = Request.Cookies["access_token"] ?? string.Empty;
            if(accessToken == string.Empty)
            {
                return Unauthorized();
            }
            return Ok();
        }

    }
}
