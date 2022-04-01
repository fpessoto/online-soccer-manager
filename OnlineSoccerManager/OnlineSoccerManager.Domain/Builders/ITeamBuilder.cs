using OnlineSoccerManager.Domain.Enums;
using OnlineSoccerManager.Domain.Teams;
using OnlineSoccerManager.Domain.Users;

namespace OnlineSoccerManager.Domain.Builders
{
    public interface ITeamBuilder
    {
        public Team Build(User user, string teamName, Country country);
    }
}