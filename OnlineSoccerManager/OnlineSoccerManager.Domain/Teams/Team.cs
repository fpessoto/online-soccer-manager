using OnlineSoccerManager.Domain.Base;
using OnlineSoccerManager.Domain.Enums;
using OnlineSoccerManager.Domain.Players;
using OnlineSoccerManager.Domain.Transfers;
using OnlineSoccerManager.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSoccerManager.Domain.Teams
{
    public class Team : Entity
    {
        public string Name { get; set; }

        public Country Country { get; set; }

        public decimal Budget { get; set; }

        public Guid OwnerId { get; set; }

        public IList<Player> Players { get; set; }

        public User Owner { get; set; }

        public IList<Transfer> Transfers { get; set; }

        public decimal Value
        {
            get
            {
                return Players?.Sum(x => x.CurrentValue) ?? 0;
            }
        }

        public void DebtBudget(decimal debtValue)
        {
            Budget -= debtValue;
        }

        public void CreditBudget(decimal creditBudget)
        {
            Budget += creditBudget;
        }
    }
}
