using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class UpdatePrimaryUserDto
    {
        public string Content { get; set; }
        public string OtherUsername { get; set; }
        public int CountUnRead { get; set; }
        public UpdatePrimaryUserDto()
        {
            
        }

        public UpdatePrimaryUserDto(string content, string otherUsername, int countUnRead)
        {
            Content = content;
            OtherUsername = otherUsername;
            CountUnRead = countUnRead;
        }
    }
}
