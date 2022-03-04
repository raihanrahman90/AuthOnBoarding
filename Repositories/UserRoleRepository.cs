using Microsoft.EntityFrameworkCore;
using TestingAuthOnBoard.Interfaces;
using TestingAuthOnBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestingAuthOnBoard.Repositories
{
    public class UserRoleRepository : BaseRepository<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(DbContext dbContext) : base(dbContext) { }

        public IQueryable<UserRole> GetUserRolesByUserId(Guid userId)
        {
            IQueryable<UserRole> result = GetAllIncluding(p => p.User).Where(p => p.UserId == userId);
            return result;
        }


        public async Task<List<UserRole>> GetUserRolesByRoleIdAsync(Guid roleId)
        {
            List<UserRole> result = await GetMany(p => p.RoleId == roleId).ToListAsync();
            return result;
        }
    }
}
