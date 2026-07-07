using Api.Extenstions;
using Application.Interfaces;
using Domain.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Api.Hubs
{
    public class PresenceHub(IPresenceTracker _tracker,IUserService userService):Hub
    {

        public async  override Task OnConnectedAsync()
        {
            var userName = Context.User.GetUserName() ?? throw new Exception("no userName Founded");
         var res=   await _tracker.UserConnected(userName, Context.ConnectionId);
                if (res)
                await Clients.Others.SendAsync("onConnectUser",Context.User?.GetUserName());

            var onlineUsers = await _tracker.GetOnlineUsers();
            await Clients.All.SendAsync("GetOnlineUsers", onlineUsers);

            var unreaded = await userService.getUnReadedMessages(userName);
            await Clients.Caller.SendAsync("GetUnReadedMessages", unreaded);
            var frindsUsers = await userService.GetUserFrinds(userName);

            await Clients.Caller.SendAsync("GetUserFrinds", frindsUsers);


        }
        public async override  Task OnDisconnectedAsync(Exception? exception)
        {
            var userName = Context.User.GetUserName() ?? throw new Exception("no userName Founded");

           var offline = await _tracker.UserDisConnected(userName, Context.ConnectionId);
            var onlineUsers = await _tracker.GetOnlineUsers();
            await Clients.All.SendAsync( "GetOnlineUsers", onlineUsers);

            if (offline)
            {
                await Clients.Others.SendAsync("onDisconnectUser", Context.User?.GetUserName());
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}
