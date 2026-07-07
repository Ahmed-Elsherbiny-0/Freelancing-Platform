using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Auth.AuthValidators
{
    internal class ConfirmationEmailRequestDtoValidator:AbstractValidator<ConfirmationEmailRequestDto>
    {
        public ConfirmationEmailRequestDtoValidator()
        {
            RuleFor(x=>x.UserId).NotEmpty();
            RuleFor(x=>x.Code).NotEmpty();
        }
    }
}
