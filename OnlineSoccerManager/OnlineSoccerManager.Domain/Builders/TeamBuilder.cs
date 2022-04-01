using OnlineSoccerManager.Domain.Enums;
using OnlineSoccerManager.Domain.Players;
using OnlineSoccerManager.Domain.Teams;
using OnlineSoccerManager.Domain.Users;

namespace OnlineSoccerManager.Domain.Builders
{
    public class TeamBuilder : ITeamBuilder
    {
        private const int DEFAULT_INITIAL_BUDGET = 5000000;
        private readonly IPlayerBuilder _playerBuilder;

        public TeamBuilder(IPlayerBuilder playerBuilder)
        {
            _playerBuilder = playerBuilder;
        }

        public Team Build(User user, string teamName, Country country)
        {
            var team = new Team
            {
                Id = Guid.NewGuid(),
                Name = teamName,
                Country = country,
                OwnerId = user.Id,
                Budget = DEFAULT_INITIAL_BUDGET,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                Players = new List<Player>()
            };

            CreateRandowPlayers(team, PlayerPosition.Goalkeeper, 3).ToList().ForEach(player =>
           {
               team.Players.Add(player);
           });
            CreateRandowPlayers(team, PlayerPosition.Defender, 6).ToList().ForEach(player =>
            {
                team.Players.Add(player);
            });
            CreateRandowPlayers(team, PlayerPosition.Midfielder, 6).ToList().ForEach(player =>
            {
                team.Players.Add(player);
            });
            CreateRandowPlayers(team, PlayerPosition.Attacker, 5).ToList().ForEach(player =>
            {
                team.Players.Add(player);
            });

            return team;

            Player[] CreateRandowPlayers(Team team, PlayerPosition position, int quantity)
            {
                Player[] players = new Player[quantity];
                for (int i = 0; i < quantity; i++)
                {
                    players[i] = _playerBuilder.Build(position, team);
                }
                return players;
            }
        }
    }
}
