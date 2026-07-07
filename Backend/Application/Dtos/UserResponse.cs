using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public record UserResponse(string FirstName,string LastName,string Photo,string Email);
}
