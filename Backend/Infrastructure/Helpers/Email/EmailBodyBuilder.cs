using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Helpers.Email
{
    public  class EmailBodyBuilder(IWebHostEnvironment env):IEmailBodyBuilder
    {
        private  readonly IWebHostEnvironment _env = env;

        public  string GenerateEmailBody(string templete, Dictionary<string, string> dictioary)
        {

            var templetePath = $"{Path.Combine(_env.ContentRootPath,"../" ,"Infrastructure")}/Services/Email/Templates/{templete}.html";
           using var stream=new StreamReader(templetePath);
            var emailbody=stream.ReadToEnd();

            foreach (var item in dictioary)
            {
            emailbody=emailbody.Replace(item.Key, item.Value);
            }

            return emailbody;
        } 
    }
}
