using DbContextDao;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class RefreshTokenRepository : EfRepository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(AuthContext dbContext) : base(dbContext)
        {
        }

        public Task<RefreshToken?> FindByAccountId(int accountId)
        {
            var result = _entities
                .FirstOrDefault(it => it.AccountId == accountId);
            return Task.FromResult(result);
        }
    }
}
