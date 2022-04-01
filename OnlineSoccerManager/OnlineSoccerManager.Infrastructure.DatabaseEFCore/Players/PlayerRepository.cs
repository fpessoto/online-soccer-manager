using Microsoft.EntityFrameworkCore;
using OnlineSoccerManager.Domain.Players;
using OnlineSoccerManager.Infrastructure.DatabaseEFCore.Context;
using System.Linq.Expressions;

namespace OnlineSoccerManager.Infrastructure.DatabaseEFCore.Players
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly OnlineSoccerDBContext _context;

        public PlayerRepository(OnlineSoccerDBContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Player entity)
        {
            await _context.Players.AddAsync(entity);
        }

        public async Task UpdateAsync(Player entity)
        {
            _context.Players.Update(entity);
        }

        public async Task DeleteAsync(Player entity)
        {
            _context.Players.Remove(entity);
        }


        public async Task<IQueryable<Player>> GetAllAsync()
        {
            return _context.Players.AsNoTracking();
        }

        public async Task<IQueryable<Player>> GetAsync(Expression<Func<Player, bool>> expression)
        {
            var PlayersQueryable = _context.Players.AsNoTracking().Where(expression).AsQueryable();

            return PlayersQueryable;
        }

        public async Task<Player> GetByIdAsync(Guid playerId)
        {

            return await _context.Players.AsNoTracking().FirstOrDefaultAsync(x => x.Id == playerId);
        }

        public async Task AddBatchAsync(IEnumerable<Player> entity)
        {
            foreach (var item in entity)
            {
                await _context.AddAsync(item);
            }
        }
    }
}
