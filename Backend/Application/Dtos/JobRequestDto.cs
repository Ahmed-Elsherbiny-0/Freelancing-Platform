using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public record JobRequestDto(
        string Title,string Description, int Budget,IList<string>?RequiredSkills,DateTime Duration 
        );
}
