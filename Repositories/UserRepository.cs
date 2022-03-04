using Microsoft.EntityFrameworkCore;
using TestingAuthOnBoard.Interfaces;
using TestingAuthOnBoard.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace TestingAuthOnBoard.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(DbContext dbContext) : base(dbContext) { }

        public async Task<User> GetByUserNameAsync(string userName)
        {
            User result = await GetSingleAsync(u => u.UserName.Equals(userName));
            return result;

        }

        public async Task<User> GetByEmailAsync(string email)
        {
            User result = await GetSingleAsync(u => u.Email.Equals(email));
            return result;
        }

        public async Task<Guid> IsAuthenticatedAsync(string userName, string password)
        {
            var user = await GetAll().Where(u => u.UserName.ToLower().Equals(userName.ToLower())).Select(x => new { x.UserId, x.Password }).SingleOrDefaultAsync();
            var enkripsi = BCrypt.Net.BCrypt.HashPassword(password);
            Console.WriteLine(enkripsi);
            if (user == default) return new Guid();
            return user.UserId;
        }
    }
}
