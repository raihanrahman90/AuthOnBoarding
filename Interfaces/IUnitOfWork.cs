using Microsoft.EntityFrameworkCore.Storage;
using TestingAuthOnBoard.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TestingAuthOnBoard.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }
        IUserRoleRepository UserRoleRepository { get; }
        IRoleRepository RoleRepository { get; }

        void Save();

        Task SaveAsync(CancellationToken cancellationToken = default(CancellationToken));

        IDbContextTransaction StartNewTransaction();

        Task<IDbContextTransaction> StartNewTransactionAsync();

        Task<int> ExecuteSqlCommandAsync(string sql, object[] parameters, CancellationToken cancellationToken = default(CancellationToken));

    }
}
