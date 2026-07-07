using Application.Dtos;
using Domain.Comman;
using Domain.Entities.UserEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public  interface IUserService
    {

        public Task<ApplicationUser> GetUserByUserNameAsync(string username);
        public  Task<IReadOnlyList<PrimaryUserChatDto>> GetUserFrinds(string username);
        public Task<IReadOnlyList<UnReadedMessageDto>> getUnReadedMessages(string userName);

        public Task<Result> UpdateUserInfo(UserInfoRequestDto request, string userId);
        public Task<Result<UserInfoRequestDto>> GetUserInfo(string userId);
    }
}
