using Domain.Entities.ChatingEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Specifications
{
    public class MessageSpecification : BaseSpecifications<Message>
    {
        public MessageSpecification(int messageId) : base(x => x.Id == messageId)
        {
            AddInclude(x => x.Sender);
            AddInclude(x => x.Recipient);
        }
        public MessageSpecification(string currentUsername, string recipientUsername) : base(m => m.Recipient.UserName == currentUsername && m.RecipientDeleted == false
                  && m.Sender.UserName == recipientUsername
                  || m.Recipient.UserName == recipientUsername
                  && m.Sender.UserName == currentUsername && m.SenderDeleted == false)
        {
            AddInclude(x => x.Recipient);
            AddThenInclude("Recipient.Photos");
            AddInclude(x => x.Sender);
            AddThenInclude("Sender.Photos");
            AddOrderBy(x => x.MessageSent);
         

        }
        public MessageSpecification(string username) : base(x => x.SenderUsername == username||x.RecipientUsername==username)
        {
            AddInclude(x => x.Sender);
            AddInclude(x => x.Recipient);
        }

    }

}