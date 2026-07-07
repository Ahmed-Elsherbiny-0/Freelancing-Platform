using Application.Dtos;
using Domain.Entities.ChatingEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IMessageService
    {
        public Task<bool> AddMessage(Message msg);
        public Task<bool> DeleteMessage(Message msg);
         public  Task<Message> GetMessage(int id);
 
        public  Task<IEnumerable<MessageDto>> GetMessagesThread(string currentUserName, string recipientUsername);
        public Task AddGroup(Group group);
        public Task RemoveConnection( Connection connection);
        public Task<Connection?> GetConnection( string  connectionId);
        public Task<Group?> GetMessageGroup( string  groupName);
        public Task<bool> SaveChangesAsync();
        public  Task<UpdatePrimaryUserDto> SendMessageOffline(string content, string username, string otheruserName);


    }
}
