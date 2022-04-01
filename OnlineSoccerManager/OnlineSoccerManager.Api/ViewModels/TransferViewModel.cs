using OnlineSoccerManager.Domain.Enums;
using OnlineSoccerManager.Domain.Players;
using OnlineSoccerManager.Domain.Teams;
using OnlineSoccerManager.Domain.Transfers;

namespace OnlineSoccerManager.Api.ViewModels
{
    public class TransferViewModel
    {
        public Guid Id { get; set; }

        public TransferPlayerViewModel Player { get; set; }

        public TransferTeamViewModel OldTeam { get; set; }

        public TransferTeamViewModel NewTeam { get; set; }

        public decimal SellPrice { get; set; }
        public Guid NewTeamId { get; set; }
        public Guid OldTeamId { get; set; }
        public Guid PlayerId { get; set; }
    }

    public class TransferTeamViewModel
    {

        public Guid Id { get; set; }

        public string Name { get; set; }

        public Country Country { get; set; }

        public List<TransferPlayerViewModel> Players { get; set; }

        public decimal Budget { get; set; }

        public decimal TeamValue { get; set; }

    }

    public class TransferPlayerViewModel
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Country Country { get; set; }

        public int Age { get; set; }

        public decimal Price { get; set; }
    }

    public static class TransferViewModelMap
    {
        public static List<TransferViewModel> ToViewModel(this List<Transfer> team)
        {
            return team.Select(x => x.ToViewModel()).ToList();
        }

        public static TransferViewModel ToViewModel(this Transfer transfer)
        {
            return new TransferViewModel
            {
                Id = transfer.Id,
                NewTeamId = transfer.NewTeamId,
                OldTeamId = transfer.OldTeamId,
                PlayerId = transfer.PlayerId,
                SellPrice = transfer.SellPrice,

                NewTeam = transfer.NewTeam?.Map(),
                OldTeam = transfer.OldTeam?.Map(),
                Player = transfer.Player?.Map(),
            };
        }

        private static TransferTeamViewModel Map(this Team team)
        {
            if (team == null) return null;
            return new TransferTeamViewModel
            {
                Id = team.Id,
                Name = team.Name,
                Country = team.Country,
                Budget = team.Budget,
                TeamValue = team.Value,
                Players = team.Players.Map()
            };
        }

        private static List<TransferPlayerViewModel> Map(this IList<Player> players)
        {
            if (players == null) return null;

            return players.Select(x => x.Map()).ToList();
        }

        private static TransferPlayerViewModel Map(this Player player)
        {
            if (player == null) return null;
            return new TransferPlayerViewModel
            {
                Age = player.Age,
                Id = player.Id,
                FirstName = player.FirstName,
                LastName = player.LastName,
                Country = player.Country,
                Price = player.CurrentValue
            };
        }
    }

}
