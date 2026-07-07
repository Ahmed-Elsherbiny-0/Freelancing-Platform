using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Auth.AuthValidators
{
    public class ResetPasswordRequestDtoValidator:AbstractValidator<ResetPasswordRequestDto>
    {
        public ResetPasswordRequestDtoValidator()
        {
            RuleFor(x => x.Email).EmailAddress().NotEmpty();
            RuleFor(x => x.NewPassword).NotEmpty();
            RuleFor(x => x.Code).NotEmpty();
        }
    }
}
