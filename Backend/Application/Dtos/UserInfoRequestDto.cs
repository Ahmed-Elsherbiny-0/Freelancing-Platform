using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public record UserInfoRequestDto(
        string FirstName,string LastName ,string Country,List<string>? Skills,string Description);
}
