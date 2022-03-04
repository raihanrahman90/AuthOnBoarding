using TestingAuthOnBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestingAuthOnBoard.Interfaces
{
    public interface IUserRoleRepository : IBaseRepository<UserRole>
    {
        IQueryable<UserRole> GetUserRolesByUserId(Guid userId);

        Task<List<UserRole>> GetUserRolesByRoleIdAsync(Guid roleId);
    }
}
