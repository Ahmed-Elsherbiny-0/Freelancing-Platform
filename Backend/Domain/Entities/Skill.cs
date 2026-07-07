using Domain.Entities.UserEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public  class Skill
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Worker? Worker { get; set; }
        public string? WorkerId { get; set; }
        public Job? Job { get; set; }
        public int? JobId { get; set; }


    }
}
