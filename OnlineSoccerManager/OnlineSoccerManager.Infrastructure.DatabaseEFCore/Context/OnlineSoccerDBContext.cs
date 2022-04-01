using Microsoft.EntityFrameworkCore;
using OnlineSoccerManager.Domain.Players;
using OnlineSoccerManager.Domain.Teams;
using OnlineSoccerManager.Domain.Transfers;
using OnlineSoccerManager.Domain.Users;
using OnlineSoccerManager.Infrastructure.DatabaseEFCore.Extensions;
using OnlineSoccerManager.Infrastructure.DatabaseEFCore.Players;
using OnlineSoccerManager.Infrastructure.DatabaseEFCore.Teams;
using OnlineSoccerManager.Infrastructure.DatabaseEFCore.Transfers;
using OnlineSoccerManager.Infrastructure.DatabaseEFCore.Users;

namespace OnlineSoccerManager.Infrastructure.DatabaseEFCore.Context
{
    public class OnlineSoccerDBContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Transfer> Transfers { get; set; }

        public OnlineSoccerDBContext(DbContextOptions options) : base(options) { 
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
            modelBuilder.ApplyConfiguration(new PlayerEntityConfiguration());
            modelBuilder.ApplyConfiguration(new TeamEntityConfiguration());
            modelBuilder.ApplyConfiguration(new TransferEntityConfiguration());

            modelBuilder.SeedUser();
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseChangeTrackingProxies(false);
            optionsBuilder.EnableDetailedErrors();
            optionsBuilder.UseLazyLoadingProxies(false);
            optionsBuilder.EnableSensitiveDataLogging();
        }

    }
}
