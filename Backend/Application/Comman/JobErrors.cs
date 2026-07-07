using Domain.Comman;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Comman
{
    public static class JobErrors
    {
        public static readonly Error NotFoundedProject =
new("Project.NotFounded", "Project not founded ", StatusCodes.Status404NotFound);
    }
}
