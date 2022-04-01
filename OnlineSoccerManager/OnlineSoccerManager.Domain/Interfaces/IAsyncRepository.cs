using OnlineSoccerManager.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSoccerManager.Domain.Interfaces
{
    public interface IAsyncRepository<T> where T : Entity
    {
        Task AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);

        Task<T> GetByIdAsync(Guid userId);

        Task<IQueryable<T>> GetAllAsync();

        Task<IQueryable<T>> GetAsync(Expression<Func<T, bool>> expression);
    }
}
