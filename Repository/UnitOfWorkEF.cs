using DbContextDao;
using Repositories;

namespace Repositories
{
    public class UnitOfWorkEF : IUnitOfWork, IAsyncDisposable
    {
        public IAccountRepository AccountRepository { get; }
        public IRoleRepository RoleRepository { get; }
        public IRefreshTokenRepository RefreshTokenRepository { get; }

        private readonly AuthContext _dbContext;

        public UnitOfWorkEF(
            AuthContext dbContext,
            IAccountRepository accountRepository,
            IRoleRepository roleRepository,
            IRefreshTokenRepository refreshTokenRepository)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            AccountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
            RoleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
            RefreshTokenRepository = refreshTokenRepository ?? throw new ArgumentNullException(nameof(refreshTokenRepository));
        }

        public Task SaveChangesAsync()
        {
            _dbContext.SaveChanges();
            return Task.CompletedTask;
        }

        public void Dispose() => _dbContext.Dispose();
        public ValueTask DisposeAsync() => _dbContext.DisposeAsync();
    }
}
