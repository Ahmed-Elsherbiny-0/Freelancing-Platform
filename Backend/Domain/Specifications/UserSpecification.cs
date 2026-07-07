using Domain.Entities.UserEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Specifications
{
    public class UserSpecification:BaseSpecifications<ApplicationUser>
    {
        public UserSpecification(string username) : base(x => x.Email == username)
        {
            AddInclude(x => x.Photos);
            AddInclude(x => x.Worker);
        }
        public UserSpecification(string userId,int id):base(x=>x.Id== userId) 
        {
            AddInclude(x => x.Photos);
            AddInclude(x => x.Client);
            AddInclude(x => x.Worker);
            AddThenInclude("Worker.Skills");
        }
    }
}
