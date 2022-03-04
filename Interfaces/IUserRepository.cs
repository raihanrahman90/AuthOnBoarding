using TestingAuthOnBoard.Interfaces;
using TestingAuthOnBoard.Models;
using System;
using System.Threading.Tasks;

namespace TestingAuthOnBoard.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User> GetByUserNameAsync(string userName);

        Task<User> GetByEmailAsync(string email);

        Task<Guid> IsAuthenticatedAsync(string userName, string password);
    }
}
