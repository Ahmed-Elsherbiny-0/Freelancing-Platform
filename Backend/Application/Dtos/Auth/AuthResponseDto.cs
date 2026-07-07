using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Auth
{
    public record AuthResponseDto
    (string Id,string Email,string FristName,string PictureUrl, string PicturePublicId, string LastName,string Token,int ExpiresIn,string refreshToken, DateTime RefreshTokenExpiration,bool IsClient);
}   
