using Models;

namespace Repositories
{
    public interface IRefreshTokenRepository : IRepository<RefreshToken>
    {
        Task<RefreshToken?> FindByAccountId(int accountId);
    }
}
