using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Comman
{
    public class Error
    {
        public Error(string code, string description,int? statusCode)
        {
            this.Code = code;
            this.Description = description;
            this.StatusCode = statusCode;
        }
        public static readonly Error None = new(String.Empty, string.Empty, null);
        public int? StatusCode { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
