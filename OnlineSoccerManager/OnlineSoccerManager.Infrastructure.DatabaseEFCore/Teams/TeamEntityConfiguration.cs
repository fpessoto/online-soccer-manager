using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineSoccerManager.Domain.Teams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSoccerManager.Infrastructure.DatabaseEFCore.Teams
{
    public class TeamEntityConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.ToTable("TB_TEAM");

            builder.HasKey(x => x.Id);

            builder.HasOne(item => item.Owner).WithOne().HasForeignKey<Team>(team => team.OwnerId).OnDelete(DeleteBehavior.NoAction);

            builder.Property(item => item.Country);
            builder.Property(item => item.Budget);
            builder.Property(item => item.Name);
            builder.Property(item => item.CreatedDate);
            builder.Property(item => item.UpdatedDate);

            builder.Ignore(item => item.Value);
            builder.Ignore(item => item.Transfers);
        }
    }
}
