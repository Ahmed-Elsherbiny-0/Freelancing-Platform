using Application.Dtos;
using Domain.Interfaces;
using Application.Interfaces;
using Domain.Specifications;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.ChatingEntities;
using Domain.Entities.UserEntities;
using Domain.Comman;
using Mapster;

namespace Infrastructure.Services
{
    public class UserService(IGenericRepository<ApplicationUser> userrepo, IGenericRepository<Message> messagerepo, ApplicationDbContext context) : IUserService
    {
        public async Task<ApplicationUser> GetUserByUserNameAsync(string username)
        {
            var res = await userrepo.GetEntityWithspec(new UserSpecification(username));
            return res;
        }
        public async Task<IReadOnlyList<PrimaryUserChatDto>> GetUserFrinds(string username)
        {

           return  await context.Database.SqlQuery<PrimaryUserChatDto>($@"
WITH UserMessages AS (
    SELECT *,
           CASE
               WHEN SenderUsername = {username} THEN RecipientUsername
               ELSE SenderUsername
           END AS OtherUsername
    FROM Messages
    WHERE {username} IN (SenderUsername, RecipientUsername)
),

RankedMessages AS (
    SELECT *,
           ROW_NUMBER() OVER (
               PARTITION BY OtherUsername
               ORDER BY MessageSent DESC
           ) AS RowNum
    FROM UserMessages
)

SELECT
    u.FirstName + ' ' + u.LastName AS FullName,
    rm.Content,rm.MessageSent,rm.OtherUsername,
	p.url
FROM RankedMessages rm
JOIN AspNetUsers u ON u.Email = rm.OtherUsername
JOIN Photos p on u.id=p.[ApplicationUserId]
WHERE rm.RowNum = 1
").ToListAsync();
            

        }

        public async Task<IReadOnlyList<UnReadedMessageDto>> getUnReadedMessages(string userName)
        {
           return await context.Messages
         .Where(m => m.DateRead == null&&userName==m.RecipientUsername)
         .GroupBy(m => new { m.RecipientUsername, m.SenderUsername })
         .Select(g => new UnReadedMessageDto(

             g.Key.SenderUsername,
             g.Count()
         ))
         .ToListAsync();

        }

        public async Task<Result> UpdateUserInfo(UserInfoRequestDto request,string userId)
        {
            var user = await userrepo.GetEntityWithspec( new UserSpecification(userId,0));
            request.Adapt(user);
            await userrepo.SaveChangesAsync();
            return Result.Success();
        }
        public async Task<Result<UserInfoRequestDto>> GetUserInfo( string userId)
        {
            var user = await userrepo.GetEntityWithspec(new UserSpecification(userId, 0));
            if (user.Client!=null)
            {
            return Result.Success(user.Adapt<UserInfoRequestDto>());

            }
            return Result.Success(user.Adapt<UserInfoRequestDto>());

        }
    }
}
