using Domain.Entities.ChatingEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Specifications
{
    public class GroupSpacification:BaseSpecifications<Group>
    {
        public GroupSpacification(string groupName):base(x=>x.Name==groupName)
        {
            AddInclude(x => x.Connections);
        }
    }
}
