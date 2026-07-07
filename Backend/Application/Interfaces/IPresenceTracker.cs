using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IPresenceTracker
    {
        public  Task<bool> UserConnected(string userName,string connectionId);
        public Task<bool> UserDisConnected(string userName,string connectionId);
        public Task<string[]> GetOnlineUsers();
        public Task<string[]> GetConnectionsForUser(string userName);
        public  Task ClearAllAsync();
    }
}
