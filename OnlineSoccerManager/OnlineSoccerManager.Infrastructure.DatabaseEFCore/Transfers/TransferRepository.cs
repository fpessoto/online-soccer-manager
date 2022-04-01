using Microsoft.EntityFrameworkCore;
using OnlineSoccerManager.Domain.Transfers;
using OnlineSoccerManager.Infrastructure.DatabaseEFCore.Context;
using System.Linq.Expressions;

namespace OnlineSoccerManager.Infrastructure.DatabaseEFCore.Transfers
{
    public class TransferRepository : ITransferRepository
    {
        private readonly OnlineSoccerDBContext _context;

        public TransferRepository(OnlineSoccerDBContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Transfer entity)
        {
            await _context.Transfers.AddAsync(entity);
        }

        public async Task UpdateAsync(Transfer entity)
        {
            _context.Transfers.Update(entity);
        }

        public async Task DeleteAsync(Transfer entity)
        {
            _context.Transfers.Remove(entity);
        }

        public async Task<IQueryable<Transfer>> GetAllAsync()
        {
            return _context.Transfers.AsNoTracking();
        }

        public async Task<IQueryable<Transfer>> GetAsync(Expression<Func<Transfer, bool>> expression)
        {
            var TransfersQueryable = _context.Transfers.AsNoTracking().Where(expression).AsNoTracking().AsQueryable();

            return TransfersQueryable;
        }

        public async Task<Transfer> GetByIdAsync(Guid TransferId)
        {

            return await _context.Transfers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == TransferId);
        }

    }
}
