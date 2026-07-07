using Application.Dtos;
using Domain.Entities;
using Domain.Entities.UserEntities;
using Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Specifications
{
    public class JobSpacification:BaseSpecifications<Job,JobResponseDto>
    {
        public JobSpacification(int id):base (x=>x.Id==id)
        {
            this.GetData();
        }
        public JobSpacification(JobSpecParms parms):base(x=>
        ((!parms.Status.HasValue)||(x.Status==parms.Status.Value))&&
        (string.IsNullOrEmpty(parms.Search) || x.Title.ToLower().Contains(parms.Search)|| x.Description.ToLower().Contains(parms.Search))&&
        (parms.Budget==null||(x.Budget>=parms.Budget))
        )
        {
            this.GetData();
            AddPagination(parms.PageSize, (parms.PageSize) * (parms.PageIndex - 1));
        }
        private void GetData()
        {
            AddInclude(x => x.Client);
            AddThenInclude("Client.User");
            AddThenInclude("Client.User.Photos");
            AddInclude(x => x.ReqiuiredSkills);
            AddOrderByDesc(x => x.CreatedDate);
            AddInclude(x => x.Offers);

            AddSelect(x => new JobResponseDto(x.Id, x.Title, x.Description, x.Budget, x.Deadline, x.ReqiuiredSkills.Select(x => x.Name).ToList(),
                 x.CreatedDate, x.Status.ToString(), x.Client.User.Photos.Select(x => x.Url).FirstOrDefault(),
                x.Client.User.FirstName, x.Client.User.LastName, x.Client.User.Email));
        }


    }
}
