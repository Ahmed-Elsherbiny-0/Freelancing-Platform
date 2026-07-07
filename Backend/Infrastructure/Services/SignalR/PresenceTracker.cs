using Application.Interfaces;
using Domain.Entities;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.SignalR
{
    public class PresenceTracker(IConnectionMultiplexer redis) : IPresenceTracker
    {
        private readonly IDatabase _database = redis.GetDatabase();
        public async Task<string[]> GetConnectionsForUser(string userName)
        {
          var res=  await _database.SetMembersAsync($"connections:{userName}");
            return res.Select(x => x.ToString()).ToArray();
        }

        public async Task<string[]> GetOnlineUsers()
        {
            var res = await _database.SetMembersAsync($"online-users");
            return res.Select(x => x.ToString()).ToArray();
        }

        public async Task<bool> UserConnected(string userName, string connectionId)
        {
            bool isOnline = false;
            if (!await _database.SetContainsAsync("online-users", userName))
            {
                isOnline=true;
            await  _database.SetAddAsync("online-users", userName);
            }
          await  _database.SetAddAsync($"connections:{userName}", connectionId);
            return  isOnline;
        }

        public async Task<bool> UserDisConnected(string userName, string connectionId)
        {
            bool isOffline = false;
            await _database.SetRemoveAsync($"connections:{userName}", connectionId);
            var res = await _database.SetLengthAsync($"connections:{userName}");
            if (res==0)
            {
                await _database.SetRemoveAsync($"online-users", userName);
                await _database.KeyDeleteAsync($"connections:{userName}");
                isOffline=true;
            }
            return isOffline;
        }

        public async Task ClearAllAsync()
        {
            await _database.KeyDeleteAsync("online-users");

            var server = redis.GetServer(redis.GetEndPoints().First());
            var connectionKeys = server.Keys(pattern: "connections:*").ToArray();

            if (connectionKeys.Length > 0)
                await _database.KeyDeleteAsync(connectionKeys);
        }
    }
}
