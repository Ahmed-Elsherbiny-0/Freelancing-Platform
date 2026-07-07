using Domain.Entities;
using Domain.Entities.ChatingEntities;
using Domain.Entities.UserEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<RefreshToken>RefreshTokens { get; set; }
        public DbSet<Message>Messages { get; set; }
        public DbSet<Group>Groups { get; set; }
        public DbSet<Connection> Connections { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Offer> Offers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
