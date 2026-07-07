using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.UserEntities
{
    public class Worker
    {
        public string? Description { get; set; }
        public decimal Rating { get; set; } = 5;
        public int CompletedTasks { get; set; } = 0;
        public IList<Skill>? Skills { get; set; } = [];
        public ApplicationUser User { get; set; }
        public string  UserId { get; set; }

        public IList<Offer>? Offers { get; set; } = [];
    }
}
