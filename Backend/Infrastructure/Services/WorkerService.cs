using Application.Comman;
using Application.Dtos;
using Application.Interfaces;
using Application.Specifications;
using Domain.Comman;
using Domain.Entities;
using Domain.Entities.UserEntities;
using Domain.Interfaces;
using Domain.Specifications;
using Mapster;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class WorkerService(IGenericRepository<Worker> workerRepo, IGenericRepository<ApplicationUser> userRepo,IGenericRepository<Offer>offerRepo, IGenericRepository<Job> jobRepo) :IWorkerService
    {
        public async Task<Result> AddOffer(OfferRequestDto request,string username)
        {
            var user = await userRepo.GetEntityWithspec(new UserSpecification(username));
            if (user == null||user.Worker==null)
            {
                return Result.Failure(UserErrors.NotFoundedUser);
            }
            var offer = request.Adapt<Offer>();
            offer.WorkerId = user.Id;
            await offerRepo.AddItem(offer);
            if (await offerRepo.SaveChangesAsync()) {

                return Result.Success();
            };
            return Result.Failure(new Error("Offer.invalidOffer", "Can not  add New offer", StatusCodes.Status400BadRequest));

        }
        public async Task<Result<OfferSpacification>> GetAllOffers(OfferSpecParms parms , int jobId)
        {
            var spec =new OfferSpacification(parms, jobId);
         return    Result.Success(spec);       
        }

        public async Task<Result<OfferSpacification>> GetoffersForUser(OfferSpecParms parms, string userid)
        {
            var spec =  new OfferSpacification( parms, userid);
            return Result.Success(spec);
        }
    }

}
