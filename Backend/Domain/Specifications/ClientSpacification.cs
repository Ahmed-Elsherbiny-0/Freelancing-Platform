using Domain.Entities.UserEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Specifications
{
    public class ClientSpacification :BaseSpecifications<Client>
    {
        public ClientSpacification(string id):base(x=>x.UserId==id)
        {
            AddInclude(x => x.User);
        }
    }
}
