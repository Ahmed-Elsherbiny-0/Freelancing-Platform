using Api.Extenstions;
using Application.Dtos;
using Application.Interfaces;
using Domain.Entities.ChatingEntities;
using Domain.Interfaces;
using Infrastructure.Services;
using Mapster;
using Microsoft.AspNetCore.SignalR;
using Org.BouncyCastle.Cms;
using StackExchange.Redis;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Api.Hubs
{
    public class MessageHub(IMessageService _messageService, IUserService _userService,IPresenceTracker _presenceTracker,IHubContext<PresenceHub>_presenceHub) : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var httpcontext = Context.GetHttpContext();
            var otherUser = httpcontext.Request.Query["user"].ToString();
            var groupName = GetGroupName(Context.User.GetUserName(), otherUser);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            var res= await AddToGroup(groupName);

            var messages = await _messageService.GetMessagesThread(Context.User.GetUserName(), otherUser);
            await Clients.Group(groupName).SendAsync("ReceiveMessageThread", messages);
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await  RemoveFromMessageGroup();

            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(CreateMessageDto messageDto)
        {
            var userName = Context.User.GetUserName();

            if (userName == messageDto.RecipientUsername)
            {
                throw new HubException("iinvalid operation:send and reciver are the same");

            }
            var sender = await _userService.GetUserByUserNameAsync(userName);
            var recipient = await _userService.GetUserByUserNameAsync(messageDto.RecipientUsername);
            if (sender == null || recipient == null)
            {
                throw new HubException("Not found user");
            }
            var message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderUsername = sender.UserName,
                RecipientUsername = recipient.UserName,
                Content = messageDto.Content,
            };
            var groupName = GetGroupName(Context.User.GetUserName(), messageDto.RecipientUsername);
          var group= await _messageService.GetMessageGroup(groupName);
            bool isoffline = false;
            string[] connections=null;
            if (group != null&&group.Connections.Any(x=>x.Username==recipient.UserName))
            {
                message.DateRead= DateTime.UtcNow;
            }
            else
            {
                isoffline = true;
                 connections = await _presenceTracker.GetConnectionsForUser(recipient.UserName);
            
            }

            var res = await _messageService.AddMessage(message);
                if (res)
                {
                    await Clients.Group(groupName).SendAsync("NewMessage", message.Adapt<MessageDto>());
                if (connections != null)
                {
                    var unreaded = await _userService.getUnReadedMessages(recipient.UserName);
                    await _presenceHub.Clients.Clients(connections).SendAsync("GetUnReadedMessages", unreaded);
                    var senderconnection = await _presenceTracker.GetConnectionsForUser(sender.UserName);
                    var allConnections = connections.Concat(senderconnection).ToArray();
                    var dto = await _messageService.SendMessageOffline(message.Content, sender.UserName, recipient.UserName);
                    await _presenceHub.Clients.Clients(allConnections).SendAsync("onReciveNewMessage", dto);
                }

            }


        }


        private async Task<bool> AddToGroup( string groupName)
        {
            var userName= Context.User?.GetUserName()??throw new Exception("Not founded user");
         var group= await  _messageService.GetMessageGroup(groupName);
            var connection=new Domain.Entities.ChatingEntities.Connection { ConnectionId=Context.ConnectionId,Username=userName};
            if (group == null)
            {
                group = new Group { Name = groupName };
              await   _messageService.AddGroup(group);
            }
            lock (group.Connections)
            {
             group.Connections.Add(connection);
            }
            return await _messageService.SaveChangesAsync();

        }

        private async Task RemoveFromMessageGroup()
        {
         var  connection= await _messageService.GetConnection(Context.ConnectionId);
            if (connection != null)
            {
               await   _messageService.RemoveConnection(connection);
                await _messageService.SaveChangesAsync();
            }

        }
        private string GetGroupName(string currentUsername, string otherUser)
        {
            var stringCompare = string.CompareOrdinal(currentUsername, otherUser);
            return stringCompare < 0 ? $"{currentUsername}-{otherUser}" : $"{otherUser}-{currentUsername}";
        }
    }
}