using Domain.Constants;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Auth.AuthValidators
{
    public class RegisterRequestDtoValidator:AbstractValidator<RegisterRequestDto>
    {
        public RegisterRequestDtoValidator()
        {
            RuleFor(x => x.Email).EmailAddress().NotEmpty();
            RuleFor(x=>x.FirstName).NotEmpty();
            RuleFor(x=>x.LastName).NotEmpty();
            RuleFor(x=>x.Country).NotEmpty();
            RuleFor(x=>x.PhoneNumber).MinimumLength(8).NotEmpty();
            RuleFor(x => x.Password)
                .NotEmpty()
                .Matches(RegexPatterns.Password)
                .WithMessage("Password should be at least 8 digits and should contains Lowercase, NonAlphanumeric and Uppercase");
        }
    }
}
