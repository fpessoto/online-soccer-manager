using OnlineSoccerManager.Domain.Enums;
using OnlineSoccerManager.Domain.Players;
using OnlineSoccerManager.Domain.Teams;
using System.Collections.Generic;

namespace OnlineSoccerManager.Api.ViewModels
{
    public class TeamViewModel
    {

        public Guid Id { get; set; }

        public string Name { get; set; }

        public Country Country { get; set; }

        public List<PlayerTeamViewModel> Players { get; set; }

        public decimal Budget { get; set; }

        public decimal TeamValue { get; set; }

    }

    public class PlayerTeamViewModel
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Country Country { get; set; }

        public int Age { get; set; }

        public decimal Price { get; set; }
        public PlayerPosition Position { get;  set; }
    }

    public static class TeamViewModelMap
    {
        #region TeamViewModel
        public static List<TeamViewModel> ToViewModel(this List<Team> team)
        {
            return team.Select(x => x.ToViewModel()).ToList();
        }

        public static TeamViewModel ToViewModel(this Team team)
        {
            return new TeamViewModel
            {
                Id = team.Id,
                Name = team.Name,
                Country = team.Country,
                Budget = team.Budget,
                TeamValue = team.Value,
                Players = team.Players.Map()
            };
        }

        private static List<PlayerTeamViewModel> Map(this IList<Player> players)
        {
            if (players == null) return null;
            return players.Select(x => x.Map()).ToList();
        }

        private static PlayerTeamViewModel Map(this Player player)
        {
            if (player == null) return null;
            return new PlayerTeamViewModel
            {
                Age = player.Age,
                Id = player.Id,
                FirstName = player.FirstName,
                LastName = player.LastName,
                Country = player.Country,
                Price = player.CurrentValue,
                Position = player.Position
            };
        }
        #endregion TeamViewModel
    }

}
