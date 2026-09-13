using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Helpers.Email
{
    public  class EmailBodyBuilder(IWebHostEnvironment env):IEmailBodyBuilder
    {
        private  readonly IWebHostEnvironment _env = env;
        public string GenerateEmailBody(string templete, Dictionary<string, string> dictioary)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"Infrastructure.Services.Email.Templates.{templete}.html";

            using var stream = assembly.GetManifestResourceStream(resourceName)
                ?? throw new FileNotFoundException($"Email template not found: {resourceName}");
            using var reader = new StreamReader(stream);
            var emailBody = reader.ReadToEnd();

            foreach (var item in dictioary)
            {
                emailBody = emailBody.Replace(item.Key, item.Value);
            }

            return emailBody;
        }
    }
}
