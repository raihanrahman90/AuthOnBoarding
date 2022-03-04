using Microsoft.EntityFrameworkCore;
using TestingAuthOnBoard.Interfaces;
using TestingAuthOnBoard.Models;

namespace TestingAuthOnBoard.Repositories
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(DbContext dbContext) : base(dbContext) { }
    }
}
