using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Auth
{
    public record RegisterRequestDto(
        string Email,
        string Password,
       string  FirstName,
       string LastName,
       string PhoneNumber,
       string Country,
   PhotoDto? Photo,
   string Type
        );
}
