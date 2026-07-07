using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Auth.AuthValidators
{
    internal class ResendConfirmationEmailRequestDtoValidator:AbstractValidator<ResendConfirmationEmailRequestDto>
    {
        public ResendConfirmationEmailRequestDtoValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
        }
    }
}
