using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public record PrimaryUserChatDto(string Content, DateTime MessageSent, string FullName, string OtherUsername,string Url);
}
