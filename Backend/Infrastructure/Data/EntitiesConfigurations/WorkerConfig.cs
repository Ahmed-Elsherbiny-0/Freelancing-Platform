using Domain.Entities.UserEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.EntitiesConfigurations
{
    public class WorkerConfig : IEntityTypeConfiguration<Worker>
    {
        public void Configure(EntityTypeBuilder<Worker> builder)
        {
            builder.HasKey(x => x.UserId);
            builder.HasMany(x=>x.Skills).WithOne(x=>x.Worker).HasForeignKey(x=>x.WorkerId);
            builder.Property(x => x.Rating).HasColumnType("decimal(3,2)").HasDefaultValue(0);
            builder.HasMany(x => x.Offers).WithOne(x => x.Worker).HasForeignKey(x => x.WorkerId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
