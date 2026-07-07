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
    public class ApplicationUserConfig : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(x => x.Email).IsRequired();
           builder.HasMany(x=>x.RefreshTokens).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex(x=>x.PhoneNumber).IsUnique();
            builder.HasOne(x=>x.Worker).WithOne(x=>x.User).HasForeignKey<Worker>(x=>x.UserId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x=>x.Client).WithOne(x=>x.User).HasForeignKey<Client>(x=>x.UserId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
