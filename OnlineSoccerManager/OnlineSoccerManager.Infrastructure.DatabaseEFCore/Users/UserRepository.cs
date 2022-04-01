using Microsoft.EntityFrameworkCore;
using OnlineSoccerManager.Domain.Users;
using OnlineSoccerManager.Infrastructure.DatabaseEFCore.Context;
using System.Linq.Expressions;

namespace OnlineSoccerManager.Infrastructure.DatabaseEFCore.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly OnlineSoccerDBContext _context;

        public UserRepository(OnlineSoccerDBContext context)
        {
            _context = context;
        }

        public async Task AddAsync(User entity)
        {
            await _context.Users.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User entity)
        {
            _context.Users.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(User entity)
        {
            _context.Users.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IQueryable<User>> GetAllAsync()
        {
            return _context.Users.AsNoTracking();
        }

        public async Task<IQueryable<User>> GetAsync(Expression<Func<User, bool>> expression)
        {
            var usersQueryable = _context.Users.AsNoTracking().Where(expression).AsQueryable();

            return usersQueryable;
        }

        public async Task<User> GetByIdAsync(Guid userId)
        {

            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == userId);
        }

        public async Task<User> GetAsync(string email, string password)
        {
            return _context.Users.AsNoTracking().FirstOrDefault(u => u.Email == email && u.Password == password);
        }
    }
}
