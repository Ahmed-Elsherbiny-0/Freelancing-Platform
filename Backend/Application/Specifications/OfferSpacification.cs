using Application.Dtos;
using Domain.Entities;
using Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Specifications
{
    public class OfferSpacification:BaseSpecifications<Offer,OfferResponseDto>
    {
        public OfferSpacification(OfferSpecParms parms,int jobid):base(x=>x.JobId==jobid) 
        {
            this.dowork( parms);

        }

        public OfferSpacification(OfferSpecParms parms, string  userId):base(x=>x.WorkerId==userId)
        {
            this.dowork(parms);
        }
        private void dowork(OfferSpecParms parms)
        {
            AddInclude(x => x.Job);
            AddInclude(x => x.Worker);
            AddThenInclude("Worker.User");
            AddThenInclude("Worker.User.Photos");
            AddPagination(parms.PageSize, (parms.PageSize) * (parms.PageIndex - 1));
            AddSelect(x => new OfferResponseDto(x.Description, x.Duration, x.Price, x.IsApproved, x.IsCompleted
                , x.JobId, x.Worker.Rating, x.Worker.CompletedTasks, x.Worker.User.Photos.Select(x => x.Url).FirstOrDefault(),
                x.Worker.User.FirstName, x.Worker.User.LastName, x.Worker.User.Email, x.Job.Title,null));
        }
    }
}
