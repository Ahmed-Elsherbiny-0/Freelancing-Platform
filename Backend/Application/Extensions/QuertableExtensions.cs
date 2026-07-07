using Domain.Entities.ChatingEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Extensions
{
    public  static  class QuertableExtensions
    {
        public static IQueryable<Message> MarkUnReadedAsReaded(this IQueryable<Message> query,string currentUserName)
        {
            var unreadedMessages=query.Where(m=>m.DateRead==null&&m.RecipientUsername==currentUserName);
            if (unreadedMessages.Any())
            {
                foreach (var item in unreadedMessages)
                {
                item.DateRead=DateTime.UtcNow;
                }
            }

            return query;
        }
    }
}
