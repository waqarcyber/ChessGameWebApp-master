using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class AccountDto
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public bool IsBanned { get; set; }
        public List<string> Roles { get; set; }

        public string ShowRoles()
        {
            var str = "";

            foreach (var role in Roles)
            {
                if (role != Roles.Last())
                    str += role + ", ";
                else
                    str += role;
            }

            return str;
        }
    }
}
