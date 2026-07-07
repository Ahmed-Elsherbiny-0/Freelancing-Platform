using Application.Dtos;
using Domain.Specifications;
using Domain.Interfaces;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Extensions;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces;
using Domain.Entities.ChatingEntities;

namespace Infrastructure.Services
{
    public class MessageService(ApplicationDbContext context,IGenericRepository<Message>messageRepo,IGenericRepository<Group>groupRepo, IGenericRepository<Connection> connectionRepo) :IMessageService
    {
        public async Task AddGroup(Group group)
        {
           await  groupRepo.AddItem(group);
            await connectionRepo.SaveChangesAsync();
        }

        public async Task<bool> AddMessage(Message msg)
        {
          await  messageRepo.AddItem(msg);
           return  await messageRepo.SaveChangesAsync();
        }
        public  async Task<bool> DeleteMessage(Message msg)
        {
             messageRepo.DeleteItem(msg);
           return  await messageRepo.SaveChangesAsync();
        }

        public async Task<Connection?> GetConnection(string connectionId)
        {
            return await context.Connections.FindAsync(connectionId);
        }

        public  async Task<Message>GetMessage (int id)
        {
           var res= await messageRepo.GetEntityWithspec(new MessageSpecification(id));
            return res;
        }

        public async Task<Group?> GetMessageGroup(string groupName)
        {
            return await groupRepo.GetEntityWithspec(new GroupSpacification(groupName));
        }

        public async Task<IEnumerable<MessageDto>> GetMessagesThread(
     string currentUserName, string recipientUsername)
        {
            var messages = await messageRepo
                .GetQuery(new MessageSpecification(currentUserName, recipientUsername))
                .ToListAsync();

            foreach (var msg in messages.Where(m => m.DateRead == null
                                                 && m.RecipientUsername == currentUserName))
            {
                msg.DateRead = DateTime.UtcNow;
            }

            await messageRepo.SaveChangesAsync();

  
            return messages.Select(m => new MessageDto
            {
                Id = m.Id,
                SenderId = m.SenderId,
                SenderUsername = m.Sender.UserName,
                RecipientId = m.RecipientId,
                RecipientUsername = m.Recipient.UserName,
                Content = m.Content,
                DateRead = m.DateRead,      
                MessageSent = m.MessageSent,
                SenderDeleted = m.SenderDeleted,
                RecipientDeleted = m.RecipientDeleted,
               RecipientPhotoUrl=m.Recipient.Photos.SingleOrDefault()?.Url,
               SenderPhotoUrl=m.Sender.Photos.SingleOrDefault()?.Url,
            });
        }

        public  async Task RemoveConnection(Connection connection)
        {
             connectionRepo.DeleteItem(connection);
             await SaveChangesAsync();

        }
        public async Task<bool> SaveChangesAsync()
        {
         return await  connectionRepo.SaveChangesAsync();
        }

        public async Task<UpdatePrimaryUserDto> SendMessageOffline(string content,string senderUsername,string recipientUsername)
        {
            var dto = new UpdatePrimaryUserDto();

            dto.CountUnRead= await context.Messages
.Where(m => m.DateRead == null && senderUsername == m.SenderUsername && recipientUsername == m.RecipientUsername)
.GroupBy(m => new { m.RecipientUsername, m.SenderUsername })
.Select(g => g.Count())
.FirstOrDefaultAsync();

            dto.Content = content;
            dto.OtherUsername = recipientUsername;
            return dto;
        }
    }
}
