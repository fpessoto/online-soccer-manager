using OnlineSoccerManager.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSoccerManager.Domain.Players
{
    public  interface IPlayerRepository : IAsyncRepository<Player>
    {
        Task AddBatchAsync(IEnumerable<Player> entity);
    }
}
