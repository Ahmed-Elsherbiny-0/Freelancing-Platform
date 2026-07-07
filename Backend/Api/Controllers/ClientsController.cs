using Api.Abstractions;
using Api.Extenstions;
using Application.Dtos;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClientsController(IClientService clientService,IGenericRepository<Job>jobRepo) : BaseApiController
    {
        [HttpPost("add-project")]
        public async Task<IActionResult> AddNewProject(JobRequestDto request)
        {
            var userId = User.GetUserId();
          var res=  await clientService.AddJob(userId, request);
           if (res.IsSuccess)
            {
                return Created();
            }
            return res.ToProblem();
        }
        [AllowAnonymous]
        [HttpGet("get-project")]
        public async Task<IActionResult> GetProject([FromQuery]int jobId)
        {
            var res = await clientService.GetJob(jobId);
            if (res.IsSuccess)
            {
                return Ok(res.Value);
            }
            return res.ToProblem();
        }

        [AllowAnonymous]
        [HttpGet("get-all-projects")]
        public async Task<IActionResult> GetAllProjects([FromQuery]JobSpecParms parms)
        {
            var res = await clientService.GetAllJobs(parms);
            if (res.IsSuccess)
            {
                return await CreatePageResult<Job, JobResponseDto>(res.Value, jobRepo, parms.PageSize, parms.PageIndex);
            }
            return res.ToProblem();
        }
        [HttpPost("approve-project")]
        public async Task<IActionResult> ApproveProject([FromQuery] string workerId, [FromQuery] int jobId)
        {
            var res = await clientService.ApproveProject(workerId, jobId);
            if (res.IsSuccess)
            {
                return NoContent();
            }
            return BadRequest("an Error happen "); 
        }
        [HttpGet("get-all-offers-for-job")]
        public async Task<IActionResult> GetOffersForJobs()
        {
            var clientId = User.GetUserId();
            var res = await clientService.GetAllWorkersForJobs(clientId);
            if (res.IsSuccess)
            {
                return Ok(res.Value);
            }
            return BadRequest("an Error happen ");
        }
        [HttpPost("complete-project")]
        public async Task<IActionResult> CompleteProject([FromQuery] string workerId, [FromQuery] int jobId)
        {
            var res = await clientService.CompleteProject(workerId, jobId);
            if (res.IsSuccess)
            {
                return NoContent();
            }
            return BadRequest("an Error happen ");
        }
    }
}
