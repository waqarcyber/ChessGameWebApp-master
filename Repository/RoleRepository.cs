using DbContextDao;
using Models;

namespace Repositories
{
    public class RoleRepository : EfRepository<Role>, IRoleRepository
    {
        public RoleRepository(AuthContext dbContext) : base(dbContext)
        {
        }

        public Task<Role?> FindByName(string name)
        {
            var result = _entities.FirstOrDefault(it => it.Name == name);
            return Task.FromResult(result);
        }
    }
}
