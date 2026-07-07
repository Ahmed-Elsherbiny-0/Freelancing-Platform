using Application.Dtos.Auth;
using Domain.Comman;
using Microsoft.AspNetCore.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAuthService
    {
        Task<Result<AuthResponseDto>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default);
        Task<Result> Register(RegisterRequestDto request, CancellationToken cancellationToken = default);
        Task<Result<AuthResponseDto>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default);
        Task<Result> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default);

        Task<Result> ConfirmEmailAsync(ConfirmationEmailRequestDto request);
        Task<Result> ResendConfirmationEmailAsync(ResendConfirmationEmailRequestDto request);
        Task<Result> SendResetPasswordCodeAsync(string email);
        Task<Result> ResetPasswordAsync(ResetPasswordRequestDto request);
    }
}
