using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IAccountRepository : IRepository<Account>
    {
        Task<Account?> FindByEmail(string email);
        Task<Account?> FindByLogin(string login);
        Task<Account?> FindByToken(string token);
        Task<List<Account>> SearchByUsername(string username);
    }
}
