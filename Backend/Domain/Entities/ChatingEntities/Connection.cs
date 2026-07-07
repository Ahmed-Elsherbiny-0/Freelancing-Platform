using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.ChatingEntities
{
    public class Connection
    {
        public required string ConnectionId { get; set; }
        public required string Username { get; set; }
    }
}
