using Application.Dtos;
using Domain.Entities.UserEntities;
using Domain.Specifications;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Specifications
{
    public class WorkerSpecification : BaseSpecifications<Worker, WorkerDto>
    {
        public WorkerSpecification(WorkerSpecParms worker) : base(x=>
        x.User.EmailConfirmed&&
        (string.IsNullOrEmpty(worker.Search)||x.User.FirstName.ToLower().Contains(worker.Search)|| x.User.LastName.ToLower().Contains(worker.Search)) &&
        (string.IsNullOrEmpty(worker.Country) || x.User.Country==worker.Country.ToLower())&&
        (worker.Rating == null  || worker.Rating <= x.Rating)
        )
        {
            AddInclude(x => x.User);
            AddOrderByDesc(x => x.Rating);
            AddOrderByDesc(x => x.CompletedTasks);
            AddOrderBy(x => x.User.FirstName);
            AddInclude(x => x.Skills);
            AddThenInclude("User.Photos");
            AddPagination(worker.PageSize, (worker.PageIndex - 1) * worker.PageSize);
            AddSelect(x => new WorkerDto(
             x.User.FirstName,
             x.User.LastName,
             x.User.Country,
             x.User.Email,
             x.User.Photos.Select(p => p.Url).FirstOrDefault()!,
             x.Rating ,                          
             x.Description ,                  
          x.Skills!=null&&x.Skills.Count > 0                    
                 ? x.Skills.Select(s => s.Name).ToList()
                 : null!,
              x.CompletedTasks  ,null                 
         ));
        }

        public WorkerSpecification(string username):base(x=>x.User.Email==username &&x.User.EmailConfirmed)
        {
            AddInclude(x => x.User);
            AddInclude(x => x.Skills);
            AddThenInclude("User.Photos");
            AddSelect(x => new WorkerDto(
            x.User.FirstName,
            x.User.LastName,
            x.User.Country,
            x.User.Email,
            x.User.Photos.Select(p => p.Url).FirstOrDefault()!,
            x.Rating,
            x.Description,
         x.Skills != null && x.Skills.Count > 0
                ? x.Skills.Select(s => s.Name).ToList()
                : null!,
             x.CompletedTasks,null
        ));
        }
    }
}
