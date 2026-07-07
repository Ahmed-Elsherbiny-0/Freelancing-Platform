using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public record JobResponseDto
   (int Id,string Title, string Description, int Budget, DateTime Deadline, IList<string>? ReqiuiredSkills,DateTime CreatedDate,string Status,
        string PhotoUrl,string FristName ,string LastName,string Email);
}
