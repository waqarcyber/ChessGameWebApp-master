using DbContextDao;
using Models;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class AccountRepository : EfRepository<Account>, IAccountRepository
    {
        public AccountRepository(AuthContext dbContext) : base(dbContext)
        {
        }

        public Task<Account?> FindByEmail(string email)
        {
            var result = _entities
                .Include(i => i.Roles)
                .Include(it => it.RefreshToken)
                .FirstOrDefault(it => it.Email == email);

            return Task.FromResult(result);
        }

        public Task<List<Account>> SearchByUsername(string username)
        {
            var result = _entities
                .Include(i => i.Roles)
                .Where(it => it.Username.Contains(username))
                .ToList();

            return Task.FromResult(result);
        }

        public Task<Account?> FindByLogin(string login)
        {
            var result = _entities
                .Include(i => i.Roles)
                .Include(it => it.RefreshToken)
                .FirstOrDefault(it => it.Login == login);

            return Task.FromResult(result);
        }

        public Task<Account?> FindByToken(string token)
        {
            var result = _entities
                .Include(i => i.Roles)
                .Include(it => it.RefreshToken)
                .FirstOrDefault(it => it.RefreshToken.Token == token);

            return Task.FromResult(result);
        }
    }
}
