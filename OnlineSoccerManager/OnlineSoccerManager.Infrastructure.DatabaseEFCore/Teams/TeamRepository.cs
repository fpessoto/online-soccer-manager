using Microsoft.EntityFrameworkCore;
using OnlineSoccerManager.Domain.Teams;
using OnlineSoccerManager.Infrastructure.DatabaseEFCore.Context;
using System.Linq.Expressions;

namespace OnlineSoccerManager.Infrastructure.DatabaseEFCore.Teams
{
    public class TeamRepository : ITeamRepository
    {
        private readonly OnlineSoccerDBContext _context;

        public TeamRepository(OnlineSoccerDBContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Team entity)
        {
            await _context.Teams.AddAsync(entity);
        }

        public async Task UpdateAsync(Team entity)
        {
            entity.Players = null;
            _context.Teams.Update(entity);
        }

        public async Task DeleteAsync(Team entity)
        {
            _context.Teams.Remove(entity);
        }

        public async Task<IQueryable<Team>> GetAllAsync()
        {
            return _context.Teams.AsNoTracking().Include(p => p.Players).AsNoTracking().Include(p => p.Owner).AsNoTracking();
        }

        public async Task<IQueryable<Team>> GetAsync(Expression<Func<Team, bool>> expression)
        {
            var TeamsQueryable = _context.Teams.AsNoTracking().Include(p => p.Players).AsNoTracking().Include(p => p.Owner).AsNoTracking().Where(expression).AsQueryable();

            return TeamsQueryable;
        }

        public async Task<Team> GetByIdAsync(Guid TeamId)
        {
            return await _context.Teams.AsNoTracking().Include(p => p.Players).AsNoTracking().Include(p => p.Owner).AsNoTracking().FirstOrDefaultAsync(t => t.Id == TeamId);
        }
    }
}
