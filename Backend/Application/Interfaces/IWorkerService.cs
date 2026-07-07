using Application.Dtos;
using Application.Specifications;
using Domain.Comman;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public  interface IWorkerService
    {
        public Task<Result> AddOffer(OfferRequestDto request, string username);
        public Task<Result<OfferSpacification>> GetAllOffers(OfferSpecParms parms, int jobId);
        public  Task<Result<OfferSpacification>> GetoffersForUser(OfferSpecParms parms, string userid);

    }
}
