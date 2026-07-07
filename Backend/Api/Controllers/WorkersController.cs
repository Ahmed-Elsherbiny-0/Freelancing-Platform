using Api.Abstractions;
using Api.Extenstions;
using Application.Dtos;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Migrations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WorkersController(IWorkerService workerService,IGenericRepository<Offer> offerRepo) : BaseApiController
    {
        [HttpPost("add-offer")]
        public async Task<IActionResult> AddOffer(OfferRequestDto request)
        {
            var username = User.GetUserName();
            var res=await workerService.AddOffer(request, username);
            if (res.IsSuccess) {
                return NoContent();
             }
            return res.ToProblem();

        }
        [AllowAnonymous]
        [HttpGet("get-offers")]
        public async Task<IActionResult> GetOffers([FromQuery]OfferSpecParms parms,[FromQuery]int jobid)
        {
          var res=  await workerService.GetAllOffers(parms,jobid);
            if (res.IsSuccess)
            {
                return await CreatePageResult<Offer, OfferResponseDto>(res.Value, offerRepo, parms.PageSize, parms.PageIndex);
            }
            return NotFound("Job Not Founded");
        }

        [HttpGet("get-worker-offers")]
        public async Task<IActionResult> GetOffersForUser([FromQuery] OfferSpecParms parms)
        {
            var userId  = User.GetUserId();

            var res = await workerService.GetoffersForUser(parms,userId);
            if (res.IsSuccess)
            {
                return await CreatePageResult<Offer, OfferResponseDto>(res.Value, offerRepo, parms.PageSize, parms.PageIndex);
            }
            return NotFound("offers Not Founded");
        }
    }
}
