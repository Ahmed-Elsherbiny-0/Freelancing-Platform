using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.ChatingEntities
{
    public class Group
    {
        [Key]
        public required string Name { get; set; }
        public IList<Connection> Connections { get; set; } = [];
    }
}
