using OnlineSoccerManager.Domain.Base;
using OnlineSoccerManager.Domain.Players;
using OnlineSoccerManager.Domain.Teams;

namespace OnlineSoccerManager.Domain.Transfers
{
    public class Transfer : Entity
    {
        public Guid PlayerId { get; set; }
        public Guid OldTeamId { get; set; }
        public Guid NewTeamId { get; set; }

        public decimal SellPrice { get; set; }

        public Player Player { get; set; }

        public Team OldTeam { get; set; }

        public Team NewTeam { get; set; }


    }
}
