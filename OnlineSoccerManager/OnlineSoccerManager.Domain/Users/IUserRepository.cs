using OnlineSoccerManager.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSoccerManager.Domain.Users
{
    public interface IUserRepository : IAsyncRepository<User>
    {
        Task<User> GetAsync(string email, string password);
    }
}
