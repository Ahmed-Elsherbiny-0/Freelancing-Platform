using Api.Abstractions;
using Api.Extenstions;
using Application.Dtos;
using Application.Interfaces;
using Domain.Entities;
using Domain.Entities.UserEntities;
using Domain.Interfaces;
using Application.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit.Cryptography;
using Application.Comman;
using Domain.Comman;

namespace Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController(IUserService userService, IPhotoService photoService,IGenericRepository<Worker>workerRepo,IGenericRepository<ApplicationUser>userRepo) : BaseApiController
    {

        [AllowAnonymous]
        [HttpPost("upload-profile-photo-cloudinary")]
        public async Task<IActionResult> UploadPhotoCloudinary(IFormFile file)
        {
            //return public id and url
            var result = await photoService.AddPhotoAsync(file);

            if (result.Error != null || result.SecureUrl == null) return BadRequest(result.Error.Message);
            
                return Ok(new PhotoDto(result.PublicId,result.SecureUrl.AbsoluteUri));
        }

        [HttpPost("add-photo-to-user")]
        public async Task<IActionResult> AddPhotoToUser(Photo photo)
        {
            //add photo to user
            var res = await photoService.AddPhotoToUser(photo, User.GetUserName());
            if (res.IsSuccess)
                return Ok(res.Value);
            return res.ToProblem();

        }


        [AllowAnonymous]
        [HttpDelete("delete-photo-cloudinary")]
        public async Task<IActionResult> DeletePhoto(string publicId)
        {
         var res=  await photoService.DeletePhotoAsync(publicId);
            if (res.Error!=null)
            return BadRequest("Cant delete this photo");
                return Ok();
        }

        [AllowAnonymous]
        [HttpGet("get-workers")]
        public async Task<IActionResult> GetWorkers([FromQuery] WorkerSpecParms parms)
        {
            var spec = new WorkerSpecification(parms);

           return await CreatePageResult<Worker,WorkerDto>(spec, workerRepo, parms.PageSize, parms.PageIndex);
        }
        [AllowAnonymous]
        [HttpGet("get-worker")]
        public async Task<IActionResult> GetWorker([FromQuery] string username)
        {
            var uaer =await userService.GetUserByUserNameAsync(username);
            if (uaer == null)
            {
                var res = Result.Failure(UserErrors.NotFoundedUser);
                return res.ToProblem() ;
            }
            var worker =await workerRepo.GetEntityWithspec(new WorkerSpecification(username));
            return Ok(worker);
        }
        [AllowAnonymous]
        [HttpGet("get-user")]
        public async Task<IActionResult> GetUser([FromQuery] string username)
        {
            var user = await userService.GetUserByUserNameAsync(username);
            if (user == null)
            {
                var res = Result.Failure(UserErrors.NotFoundedUser);
                return res.ToProblem();
            }
            var userResponse=new UserResponse(user.FirstName, user.LastName,user.Photos.Select(x=>x.Url).FirstOrDefault(),user.Email);
            return Ok(userResponse);
        }


        [HttpPost("update-user-Info")]
        public async Task<IActionResult> UpdateUserInfo(UserInfoRequestDto request)
        {
            var res = await userService.UpdateUserInfo(request,User.GetUserId());
            if (res.IsSuccess)
            {
                return Created();
            }
            return BadRequest(res.Error);
        }
        [HttpGet("get-user-Info")]
        public async Task<IActionResult> GetUserInfo()
        {
            var res = await userService.GetUserInfo( User.GetUserId());
            if (res.IsSuccess)
            {
                return Ok(res.Value);
            }
            return BadRequest(res.Error);
        }

    }
}
