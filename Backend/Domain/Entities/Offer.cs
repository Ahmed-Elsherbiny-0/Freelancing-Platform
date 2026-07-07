using Domain.Entities.UserEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public  class Offer
    {
        public string Description { get; set; }
        public DateTime Duration { get; set; }
        public int Price { get; set; }
        public bool IsApproved { get; set; } = false;
        public bool IsCompleted { get; set; }=false;
        public Worker Worker { get; set; }
        public string WorkerId {  get; set; }
        public Job Job { get; set; }
        public int JobId { get; set; }
    }
}
