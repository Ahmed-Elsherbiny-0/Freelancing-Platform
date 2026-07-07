using Domain.Entities.Enum;
using Domain.Entities.UserEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Job
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }= DateTime.UtcNow;
        public JobStatus Status { get; set; } = JobStatus.Pending;
        public IList<Skill> ReqiuiredSkills { get; set; } = [];
        public Client Client {  get; set; }
        public string ClientId {  get; set; }
        public int Budget { get; set; }
        public DateTime Deadline { get; set; }
        public IList<Offer>? Offers { get; set; } = [];
    }
}
