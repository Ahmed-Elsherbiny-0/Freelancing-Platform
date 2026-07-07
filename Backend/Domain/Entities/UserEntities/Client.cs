using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.UserEntities
{
    public  class Client
    {
        public IList<Job>? Jobs { get; set; } = [];
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
    }
}
