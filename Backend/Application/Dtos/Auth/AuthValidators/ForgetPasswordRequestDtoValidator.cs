using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Auth.AuthValidators
{
    public class ForgetPasswordRequestDtoValidator:AbstractValidator<ForgetPasswordRequestDto>
    {
        public ForgetPasswordRequestDtoValidator()
        {
            RuleFor(x => x.Email).EmailAddress().NotEmpty();
        }
    }
}
