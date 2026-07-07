using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public record WorkerDto(string FirstName,string LastName,string Country,string Email
        ,string PhotoUrl,decimal Rating,string Description
        ,List<string>?Skills,int CompletedTasks,int? jobId);
}
