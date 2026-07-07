using Application.Dtos;
using Application.Dtos.Auth;
using Domain.Constants;
using Domain.Entities;
using Domain.Entities.UserEntities;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mapping
{
    public class MappingConfiguration : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig< RegisterRequestDto,ApplicationUser>()
            .Map(y=>y.UserName,x => x.Email)
            .Map(x => x.Photos, y => new List<Photo>() { y.Photo.Adapt<Photo>() });

            config.NewConfig<JobRequestDto, Job>()
               .Map(x => x.ReqiuiredSkills, y => y.RequiredSkills == null ? new List<Skill>() : y.RequiredSkills.Select(x => new Skill { Name = x }).ToList());

            config.NewConfig<ApplicationUser, UserInfoRequestDto>().Map(x => x.Description, y => y.Worker.Description)
                .Map(x=>x.Skills,y=>y.Worker.Skills.Select(x=>x.Name).ToList());
        }
    }
}
