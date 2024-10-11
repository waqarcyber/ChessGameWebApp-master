﻿using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IRoleRepository : IRepository<Role>
    {
        Task<Role?> FindByName(string name);
    }
}
