using Application.Comman;
using Application.Dtos;
using Application.Interfaces;
using Application.Specifications;
using Domain.Comman;
using Domain.Entities;
using Domain.Entities.Enum;
using Domain.Entities.UserEntities;
using Domain.Interfaces;
using Domain.Specifications;
using Infrastructure.Data;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public  class ClientService(IGenericRepository<Client>clientRepo, IGenericRepository<Job> jobRepo,ApplicationDbContext context) : IClientService
    {
       public async Task<Result> AddJob(string userId,JobRequestDto request)
        {
            var client = await clientRepo.GetEntityWithspec(new ClientSpacification(userId));
            if (client == null)
            {
                return Result.Failure(UserErrors.NotFoundedUser);
            }

            var job = request.Adapt<Job>();
            job.Client = client;
            job.ClientId = client.UserId;
            await jobRepo.AddItem(job);
            await jobRepo.SaveChangesAsync();

            return Result.Success(client);
        } 
    
        public async Task<Result<JobResponseDto?>> GetJob(int jobid)
        {
            var job = await jobRepo.GetEntityWithspec(new JobSpacification(jobid));
            if (job == null) {
              return  Result.Failure<JobResponseDto>(JobErrors.NotFoundedProject);
            }
            return Result.Success(job);
        }

        public async Task<Result<JobSpacification>> GetAllJobs(JobSpecParms parms)
        {
            var jobs = new JobSpacification(parms);
            return Result.Success(jobs);
        }
   
        public async Task<Result> ApproveProject(string workerId, int jobId)
        {

            await context.Offers
          .Where(x => x.JobId == jobId)
          .ExecuteUpdateAsync(s => s
              .SetProperty(x => x.IsApproved, false).SetProperty(x => x.IsCompleted, true)
          );

            await context.Offers
                .Where(x => x.JobId == jobId && x.WorkerId == workerId)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(x => x.IsApproved, true)
                    .SetProperty(x=>x.IsCompleted,false)
                    );

            await context.Jobs.Where(x=>x.Id==jobId).ExecuteUpdateAsync(x=>
                 x.SetProperty(x => x.Status, JobStatus.Closed)
                );
            return Result.Success();
        }

        public async Task<Result> CompleteProject(string workerId, int jobId)
        {
            var record = await context.Offers.Include(x=>x.Worker).Where(x => x.JobId == jobId && x.IsApproved).SingleOrDefaultAsync();

            record.IsCompleted = true;
            record.Worker.CompletedTasks = record.Worker.CompletedTasks + 1;
            context.Update(record);
            await context.SaveChangesAsync();
            return Result.Success();
        }
        public async Task<Result<List<OfferResponseDto>>> GetAllWorkersForJobs(string clientId)
        {
           return Result.Success( await context.Offers.Include(x=>x.Job).Include(x=>x.Worker).ThenInclude(x=>x.User)
             .Where(x=>x.Job.ClientId==clientId).OrderBy(x=>x.JobId).Select(x=>new OfferResponseDto(
                x.Description,
                x.Duration,
                x.Price,
                x.IsApproved,
                x.IsCompleted,
                x.JobId,
                x.Worker.Rating,
                x.Worker.CompletedTasks,
                x.Worker.User.Photos.Select(x=>x.Url).FirstOrDefault(),
                x.Worker.User.FirstName,
                x.Worker.User.LastName,
                x.Worker.User.Email,
                null,
                x.Worker.User.Id
                )).ToListAsync());
        }
    }
}
