using Application.Comman;
using Application.Dtos;
using Application.Dtos.Auth;
using Application.Interfaces;
using Domain.Comman;
using Domain.Constants;
using Domain.Entities;
using Domain.Entities.UserEntities;
using Infrastructure.Data;
using Infrastructure.Helpers;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MimeKit.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Auth
{
    public class AuthService(SignInManager<ApplicationUser> _signInManager,
        ApplicationDbContext _context,
        IJwtProvider _jwtProvider
        ,ILogger<AuthService>_logger,
        IHttpContextAccessor _httpContextAccessor,
        IEmailSender _emailSender,
        IEmailBodyBuilder _emailBodyBuilder,
        IPhotoService _photoService) : IAuthService
    {
        private readonly int _refreshTokenExpiryDays = 14;
        public async Task<Result<AuthResponseDto>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
        {
            var user = await _signInManager.UserManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Result.Failure<AuthResponseDto>(UserErrors.InvalidCredentials);
            }
   
            if (user.IsDisabled)
                return Result.Failure<AuthResponseDto>(UserErrors.DisabledUser);

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, true);
            if (result.Succeeded)
            {
                ///to do Roles
                var (token, expire) = _jwtProvider.GenerateToken(user, [], []);
                var refreshToken = GenerateRefreshToken();
                var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);
                user.RefreshTokens.Add(new RefreshToken
                {
                    Token = refreshToken,
                    ExpiresOn = refreshTokenExpiration
                });
                _context.Update(user);
                await _context.SaveChangesAsync(cancellationToken);
               var userWithPhoto= await _context.Users.Include(x => x.Photos).Include(x=>x.Worker).FirstOrDefaultAsync(x => x.Email == email);
                var photo =  userWithPhoto?.Photos.FirstOrDefault();
                var response = new AuthResponseDto(user.Id, user.Email!,user.FirstName,photo?.Url,photo?.PublicId, user.LastName, token, expire,refreshToken,refreshTokenExpiration,user.Worker==null);
                var accessTokenCookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddYears(expire)
                };
                _httpContextAccessor.HttpContext?.Response.Cookies.Append("access_token", token, accessTokenCookieOptions);

                return Result.Success(response);
            }
            var error = result.IsLockedOut ? UserErrors.LockedUser : result.IsNotAllowed ? UserErrors.EmailNotConfirmed : UserErrors.InvalidCredentials;
            return Result.Failure<AuthResponseDto>(error);
        }


        public async Task<Result>Register(RegisterRequestDto request, CancellationToken cancellationToken=default)
        {
          var userisFind = await  _signInManager.UserManager.FindByEmailAsync(request.Email);
            if (userisFind != null)
            {
                return Result.Failure(UserErrors.DuplicatedEmail);
            }
            var phone = await _context.Users.SingleOrDefaultAsync(x=>x.PhoneNumber==request.PhoneNumber);
            if (phone != null) { 
                return Result.Failure(UserErrors.DuplicatedPhone);
            }
           if (request.Photo == null)
            {
                request = request with { Photo = new PhotoDto(PhotoConstant.publicId, PhotoConstant.url) };
            }

  
            var user= request.Adapt<ApplicationUser>();
          
             var result=   await _signInManager.UserManager.CreateAsync(user,request.Password);
              var photoResult=  await _photoService.AddPhotoToUser(request.Photo.Adapt<Photo>(),user.Email);
            if (request.Type != "client" && request.Type != "worker")
            {
                return Result.Failure(UserErrors.InvalidCredentials);
            }
            if ( result.Succeeded&&photoResult.IsSuccess)
            {
                // todo add role and type
                if (request.Type== "worker")
                {
                    var worker = new Worker
                    {
                        UserId = user.Id
                    };
                   await  _context.Workers.AddAsync(worker);
                    await _context.SaveChangesAsync();  
                }
                if (request.Type == "client")
                {
                    var client = new Client
                    {
                        UserId = user.Id
                    };
                    await _context.Clients.AddAsync(client);
                    await _context.SaveChangesAsync();
                }
                var code = await _signInManager.UserManager.GenerateEmailConfirmationTokenAsync(user);

                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                _logger.LogInformation("Confirmation code: {code}", code);

                await SendConfirmationEmail(user, code);
                return Result.Success();
            }

            var error = result.Errors.First();

            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));

        }


        public async Task<Result<AuthResponseDto>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
        {
         var res=  _jwtProvider.ValidateToken(token);
            if (res == null)
            {
                return Result.Failure<AuthResponseDto>(UserErrors.InvalidJwtToken);
            }
            var user =await _context.Users.Include(x=>x.Photos).Include(x=>x.Worker).Include(x => x.RefreshTokens).FirstOrDefaultAsync(x=>x.Id==res);
            if (user == null)
            {
                return Result.Failure<AuthResponseDto>(UserErrors.InvalidJwtToken);
            }
            if (user.IsDisabled)
            {
                return Result.Failure<AuthResponseDto>(UserErrors.DisabledUser);
            }
            if (user.LockoutEnd > DateTime.UtcNow)
            {
                return Result.Failure<AuthResponseDto>(UserErrors.LockedUser);
            }

            var oldRefreshToken =user.RefreshTokens.FirstOrDefault(x => x.Token == refreshToken && x.IsActive);
            if (oldRefreshToken == null) {
                return Result.Failure<AuthResponseDto>(UserErrors.InvalidRefreshToken);
            }

            oldRefreshToken.RevokeOn=DateTime.UtcNow;
            var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);
             
            var  newRefreshToken = GenerateRefreshToken();
            var addRefreshtoken = new RefreshToken
            {
                ExpiresOn = refreshTokenExpiration,
                Token = newRefreshToken,  
            };
            //to do
            var (newToken, expiresIn) = _jwtProvider.GenerateToken(user, [], []);

            user.RefreshTokens.Add(addRefreshtoken);
            _context.Update(user);
           await  _context.SaveChangesAsync();
            var photo = user.Photos.FirstOrDefault();
            var response = new AuthResponseDto(user.Id, user.Email!, user.FirstName, photo?.Url,photo?.PublicId, user.LastName, token, expiresIn, newRefreshToken, refreshTokenExpiration,user.Worker==null);

            var accessTokenCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddSeconds(expiresIn)
            };
            _httpContextAccessor.HttpContext?.Response.Cookies.Append("access_token", token, accessTokenCookieOptions);
            return Result.Success(response);

        }

        public async Task<Result> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
        {
            var userId = _jwtProvider.ValidateToken(token);

            if (userId is null)
                return Result.Failure(UserErrors.InvalidJwtToken);

            var user = await _context.Users.Include(x=>x.RefreshTokens).FirstOrDefaultAsync(x=>x.Id==userId);

            if (user is null)
                return Result.Failure(UserErrors.InvalidJwtToken);

            var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.IsActive);

            if (userRefreshToken is null)
                return Result.Failure(UserErrors.InvalidRefreshToken);

            userRefreshToken.RevokeOn = DateTime.UtcNow;

            await _signInManager.UserManager.UpdateAsync(user);


            return Result.Success();
        }


        public async Task<Result> ConfirmEmailAsync(ConfirmationEmailRequestDto request)
        {
            if (await _signInManager.UserManager.FindByIdAsync(request.UserId) is not { } user)
                return Result.Failure(UserErrors.InvalidCode);

            if (user.EmailConfirmed)
                return Result.Failure(UserErrors.DuplicatedEmail);

            var code = request.Code;

            try
            {
                code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            }
            catch (FormatException)
            {
                return Result.Failure(UserErrors.InvalidCode);
            }

            var result = await _signInManager.UserManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
            {
               // todo
                //await _signInManager.UserManager.AddToRoleAsync(user, DefaultRoles.Member.Name);
                return Result.Success();
            }

            var error = result.Errors.First();

            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }

        public async Task<Result> ResendConfirmationEmailAsync(ResendConfirmationEmailRequestDto request)
        {
            if (await _signInManager.UserManager.FindByEmailAsync(request.Email) is not { } user)
                return Result.Success();

            if (user.EmailConfirmed)
                return Result.Failure(UserErrors.DuplicatedConfirmation);

            var code = await _signInManager.UserManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            _logger.LogInformation("Confirmation code: {code}", code);

            await SendConfirmationEmail(user, code);

            return Result.Success();
        }

        public async Task<Result> SendResetPasswordCodeAsync(string email)
        {
            if (await _signInManager.UserManager.FindByEmailAsync(email) is not { } user)
                return Result.Success();

            if (!user.EmailConfirmed)
                return Result.Failure(UserErrors.EmailNotConfirmed);

            var code = await _signInManager.UserManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            _logger.LogInformation("Reset code: {code}", code);

            await SendResetPasswordEmail(user, code);

            return Result.Success();
        }

        public async Task<Result> ResetPasswordAsync(ResetPasswordRequestDto request)
        {
            var user = await _signInManager.UserManager.FindByEmailAsync(request.Email);

            if (user is null || !user.EmailConfirmed)
                return Result.Failure(UserErrors.InvalidCode);

            IdentityResult result;

            try
            {
                var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));
                result = await _signInManager.UserManager.ResetPasswordAsync(user, code, request.NewPassword);
            }
            catch (FormatException)
            {
                result = IdentityResult.Failed(_signInManager.UserManager.ErrorDescriber.InvalidToken());
            }

            if (result.Succeeded)
                return Result.Success();

            var error = result.Errors.First();

            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status401Unauthorized));
        }


        private async Task SendConfirmationEmail(ApplicationUser user, string code)
        {
            var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;
           
            var emailBody = _emailBodyBuilder.GenerateEmailBody("EmailConfirmation", new Dictionary<string, string>
            {
                { "{{name}}", user.FirstName },
                    { "{{action_url}}", $"{origin}/verify-email?userId={user.Id}&code={code}" }
            });

         
            await _emailSender.SendEmailAsync(user.Email, "✅ Mostaqel: Email Confirmation", emailBody);
            await Task.CompletedTask;    
        }


        private async Task SendResetPasswordEmail(ApplicationUser user, string code)
        {
            var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;

            var emailBody = _emailBodyBuilder.GenerateEmailBody("ForgetPassword",
                 new Dictionary<string, string>
                {
                { "{{name}}", user.FirstName },
                { "{{action_url}}", $"{origin}/reset-password?email={user.Email}&code={code}" }
                }
            );

           await _emailSender.SendEmailAsync(user.Email!, "✅ Mostaqel: Change Password", emailBody);

            await Task.CompletedTask;
        }

        private static string GenerateRefreshToken()
        {
            return WebEncoders.Base64UrlEncode(RandomNumberGenerator.GetBytes(64));
        }

  

   
    }
}