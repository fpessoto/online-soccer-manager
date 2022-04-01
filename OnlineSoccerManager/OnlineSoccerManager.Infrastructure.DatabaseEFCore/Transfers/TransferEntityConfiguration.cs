using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineSoccerManager.Domain.Transfers;

namespace OnlineSoccerManager.Infrastructure.DatabaseEFCore.Transfers
{
    public class TransferEntityConfiguration : IEntityTypeConfiguration<Transfer>
    {
        public void Configure(EntityTypeBuilder<Transfer> builder)
        {
            builder.ToTable("TB_TRANSFER");

            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Player).WithMany().HasForeignKey(x => x.PlayerId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.NewTeam).WithMany().HasForeignKey(x => x.NewTeamId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.OldTeam).WithMany().HasForeignKey(x => x.OldTeamId).OnDelete(DeleteBehavior.NoAction);

            builder.Property(x => x.SellPrice);
            builder.Property(item => item.CreatedDate);
            builder.Property(item => item.UpdatedDate);
        }
    }
}
