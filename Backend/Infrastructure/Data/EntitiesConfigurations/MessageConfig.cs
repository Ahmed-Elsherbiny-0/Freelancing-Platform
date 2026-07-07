using Domain.Entities.ChatingEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.EntitiesConfigurations
{
    public class MessageConfig : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.HasOne(x=>x.Sender).WithMany().HasForeignKey(x=>x.SenderId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x=>x.Recipient).WithMany().HasForeignKey(x=>x.RecipientId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
