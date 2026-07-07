using Application.Dtos;
using Application.Specifications;
using Domain.Comman;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IClientService
    {
        public  Task<Result> AddJob(string userId, JobRequestDto request);
        public  Task<Result<JobResponseDto?>> GetJob(int jobid);
        public Task<Result<JobSpacification>> GetAllJobs(JobSpecParms parms);
        public Task<Result> ApproveProject(string workerId, int jobId);
        public Task<Result<List<OfferResponseDto>>> GetAllWorkersForJobs(string clientId);
        public  Task<Result> CompleteProject(string workerId, int jobId);
    }
}
