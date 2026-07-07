using Domain.Constants;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Auth.AuthValidators
{
    public class LoginRequestDtoValidator:AbstractValidator<LoginRequestDto>
    {
        public LoginRequestDtoValidator()
        {
            RuleFor(x => x.Email).EmailAddress().NotEmpty();
            RuleFor(x => x.Password)
               .NotEmpty()
               .Matches(RegexPatterns.Password)
               .WithMessage("Password should be at least 8 digits and should contains Lowercase, NonAlphanumeric and Uppercase");
        }
    }
}
