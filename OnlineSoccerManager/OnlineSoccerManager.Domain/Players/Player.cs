using OnlineSoccerManager.Domain.Base;
using OnlineSoccerManager.Domain.Enums;
using OnlineSoccerManager.Domain.Teams;
using OnlineSoccerManager.Domain.Transfers;

namespace OnlineSoccerManager.Domain.Players
{
    public class Player : Entity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Country Country { get; set; }

        public int Age { get; set; }

        public bool IsOnTransferList { get; private set; }

        public decimal AskPrice { get; private set; }

        public decimal CurrentValue { get; set; }

        public PlayerPosition Position { get; set; }

        public Guid TeamId { get; set; }

        public Team Team { get; set; }

        public IList<Transfer> Transfers { get; set; }

        public Player()
        {
            IsOnTransferList = false;
        }

        public void SetToTransferList(decimal askPrice)
        {
            if (askPrice <= 0) throw new ArgumentException("askPrice should be greather than zero.");
            IsOnTransferList = true;
            AskPrice = askPrice;
        }

        public void RemoveFromTransferList()
        {
            IsOnTransferList = false;
            AskPrice = 0;
        }

        public void Transfer(Team newTeam)
        {
            TeamId = newTeam.Id;
            CurrentValue *= 1 + Convert.ToDecimal(new Random().Next(1, 100)) / 100;
        }
    }

    public enum PlayerPosition : int
    {
        Goalkeeper,
        Defender,
        Midfielder,
        Attacker
    }
}
