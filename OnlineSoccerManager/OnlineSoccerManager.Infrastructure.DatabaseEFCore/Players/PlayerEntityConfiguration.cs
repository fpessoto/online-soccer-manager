using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineSoccerManager.Domain.Players;
using OnlineSoccerManager.Domain.Teams;

namespace OnlineSoccerManager.Infrastructure.DatabaseEFCore.Players
{
    public class PlayerEntityConfiguration : IEntityTypeConfiguration<Player>
    {
        public void Configure(EntityTypeBuilder<Player> builder)
        {
            builder.ToTable("TB_PLAYER");

            builder.HasKey(x => x.Id);
            //builder.HasOne(item => item.Team).WithMany(x => x.Players).
            //    HasForeignKey(player => player.TeamId)
            //    .OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(x => x.TeamId);

            builder.Property(item => item.IsOnTransferList);
            builder.Property(item => item.CurrentValue);
            builder.Property(item => item.Age);
            builder.Property(item => item.AskPrice);
            builder.Property(item => item.Country);
            builder.Property(item => item.FirstName);
            builder.Property(item => item.LastName);
            builder.Property(item => item.CreatedDate);
            builder.Property(item => item.UpdatedDate);

            builder.Ignore(item => item.Transfers);
            builder.Ignore(item => item.Team);
        }
    }
}
