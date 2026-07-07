using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class RefreshToken
    {
        public int id { get; set; }
        public string Token { get; set; }
        public DateTime? RevokeOn { get; set; }
        public DateTime ExpiresOn { get; set; }
        public DateTime CreatedOn { get; set; }=DateTime.UtcNow;
        public bool IsExpire => DateTime.UtcNow >= ExpiresOn;
        public bool IsActive => !IsExpire&&RevokeOn is null;
    }
}
