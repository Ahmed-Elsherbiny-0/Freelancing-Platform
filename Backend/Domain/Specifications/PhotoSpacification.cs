using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Specifications
{
    public  class PhotoSpacification:BaseSpecifications<Photo>
    {
        public PhotoSpacification(string id):base(x=>x.PublicId==id)
        {
        }
    }
}
