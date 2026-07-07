using Domain.Comman;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Comman
{
    public static  class UserErrors
    {
        public static readonly Error InvalidCredentials =
      new("User.InvalidCredentials", "Invalid email/password", StatusCodes.Status401Unauthorized);

        public static readonly Error DisabledUser =
        new("User.DisabledUser", "Disabled user, please contact your administrator", StatusCodes.Status401Unauthorized);

        public static readonly Error LockedUser =
      new("User.LockedUser", "Locked user, please contact your administrator", StatusCodes.Status401Unauthorized);
        public static readonly Error EmailNotConfirmed =
       new("User.EmailNotConfirmed", "Email is not confirmed", StatusCodes.Status401Unauthorized);

        public static readonly Error DuplicatedEmail =
       new("User.DuplicatedEmail", "Another user with the same email is already exists", StatusCodes.Status409Conflict);

        public static readonly Error DuplicatedPhone =
  new("User.DuplicatedPhone", "Another user with the same Phone is already exists", StatusCodes.Status409Conflict);
        public static readonly Error InvalidJwtToken =
        new("User.InvalidJwtToken", "Invalid Jwt token", StatusCodes.Status401Unauthorized);
        public static readonly Error InvalidRefreshToken =
      new("User.InvalidRefreshToken", "Invalid refresh token", StatusCodes.Status401Unauthorized);
        public static readonly Error InvalidCode =
    new("User.InvalidCode", "Invalid code", StatusCodes.Status401Unauthorized);

        public static readonly Error DuplicatedConfirmation =
    new("User.DuplicatedConfirmation", "Email already confirmed", StatusCodes.Status400BadRequest);
        public static readonly Error NotFoundedUser=
new("User.NotFounded", "User not founded ", StatusCodes.Status404NotFound);

    }
}
