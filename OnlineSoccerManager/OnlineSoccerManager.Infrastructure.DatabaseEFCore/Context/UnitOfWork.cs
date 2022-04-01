using OnlineSoccerManager.Domain.Interfaces;

namespace OnlineSoccerManager.Infrastructure.DatabaseEFCore.Context
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly OnlineSoccerDBContext _context;

        public UnitOfWork(OnlineSoccerDBContext context)
        {
            _context = context;
        }

        public async Task BeginTransactionAsync()
        {
            await _context.Database.BeginTransactionAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
            await _context.Database.CommitTransactionAsync();
        }
    }
}
