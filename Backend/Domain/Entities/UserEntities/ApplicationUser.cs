using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.UserEntities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; }=string.Empty;
        public string Country { get; set; }=string.Empty;
        public bool IsDisabled { get; set; }
        public IList<RefreshToken> RefreshTokens { get; set; } =[];
        public IList<Photo> Photos { get; set; } =[];
        public Client? Client { get; set; }
        public Worker? Worker { get; set; }

    }
}
