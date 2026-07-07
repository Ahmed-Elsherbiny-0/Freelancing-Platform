using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Helpers
{
    public interface IEmailBodyBuilder
    {
        public string GenerateEmailBody(string templete, Dictionary<string, string> dictioary);

    }
}
