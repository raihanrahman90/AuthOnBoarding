using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TestingAuthOnBoard.Interfaces;
using TestingAuthOnBoard.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TestingAuthOnBoard.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly AuthDbContext dbContext;

        public IUserRepository UserRepository { get; }
        public IUserRoleRepository UserRoleRepository { get; }
        public IRoleRepository RoleRepository { get; }


        public UnitOfWork(AuthDbContext context)
        {
            dbContext = context;

            UserRepository = new UserRepository(context);
            UserRoleRepository = new UserRoleRepository(context);
            RoleRepository = new RoleRepository(context);
        }

        public void Save()
        {
            dbContext.SaveChanges();
        }

        public Task SaveAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return dbContext.SaveChangesAsync(cancellationToken);
        }

        public IDbContextTransaction StartNewTransaction()
        {
            return dbContext.Database.BeginTransaction();
        }

        public Task<IDbContextTransaction> StartNewTransactionAsync()
        {
            return dbContext.Database.BeginTransactionAsync();
        }

        public Task<int> ExecuteSqlCommandAsync(string sql, object[] parameters, CancellationToken cancellationToken = default(CancellationToken))
        {
            return dbContext.Database.ExecuteSqlRawAsync(sql, parameters, cancellationToken);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    dbContext?.Dispose();
                }
                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
