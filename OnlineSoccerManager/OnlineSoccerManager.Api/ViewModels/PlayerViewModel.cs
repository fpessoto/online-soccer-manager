using OnlineSoccerManager.Domain.Enums;
using OnlineSoccerManager.Domain.Players;
using OnlineSoccerManager.Domain.Teams;

namespace OnlineSoccerManager.Api.ViewModels
{
    public class PlayerViewModel
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Country Country { get; set; }

        public int Age { get; set; }

        public decimal Price { get; set; }

        public TeamPlayerViewModel Team { get; set; }
        public bool IsOnTransferList { get; internal set; }
        public Guid TeamId { get; internal set; }
    }

    public class TeamPlayerViewModel
    {

        public Guid Id { get; set; }

        public string Name { get; set; }

        public Country Country { get; set; }

        public decimal Budget { get; set; }

        public decimal TeamValue { get; set; }
    }

    public static class PlayerViewModelMap
    {
        #region PlayerViewModel
        public static List<PlayerViewModel> ToViewModel(this IList<Player> players)
        {
            return players.Select(x => x.ToViewModel()).ToList();
        }

        public static PlayerViewModel ToViewModel(this Player player)
        {
            return new PlayerViewModel
            {
                Age = player.Age,
                Id = player.Id,
                FirstName = player.FirstName,
                LastName = player.LastName,
                Country = player.Country,
                Price = player.CurrentValue,
                IsOnTransferList = player.IsOnTransferList,
                TeamId = player.TeamId,
                Team = player.Team?.Map()
            };
        }

        private static TeamPlayerViewModel Map(this Team team)
        {
            if (team == null) return null;

            return new TeamPlayerViewModel
            {
                Id = team.Id,
                Name = team.Name,
                Country = team.Country,
                Budget = team.Budget,
                TeamValue = team.Value,
            };
        }
        #endregion
    }
}
