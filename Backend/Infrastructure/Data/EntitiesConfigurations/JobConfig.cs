using Domain.Entities;
using Domain.Entities.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.EntitiesConfigurations
{
    internal class JobConfig : IEntityTypeConfiguration<Job>
    {
        public void Configure(EntityTypeBuilder<Job> builder)
        {
            builder.Property(x => x.Status).HasConversion(x => x.ToString(), y => (JobStatus)Enum.Parse(typeof(JobStatus),y));
            builder.HasMany(x=>x.ReqiuiredSkills).WithOne(x=>x.Job).HasForeignKey(x=>x.JobId);
            builder.HasMany(x => x.Offers).WithOne(x => x.Job).HasForeignKey(x => x.JobId);
        }
    }
}
