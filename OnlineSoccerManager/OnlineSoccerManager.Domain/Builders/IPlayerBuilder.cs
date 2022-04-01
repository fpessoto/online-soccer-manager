using OnlineSoccerManager.Domain.Players;
using OnlineSoccerManager.Domain.Teams;

namespace OnlineSoccerManager.Domain.Builders
{
    public interface IPlayerBuilder
    {
        public Player Build(PlayerPosition position, Team team);
    }
}