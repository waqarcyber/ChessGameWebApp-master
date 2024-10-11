using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGameClient.Models
{
    public class UserInfo
    {
        public int AccountId { get; set; }
        public string UserName { get; set; }
        public List<string> Roles { get; set; }
    }
}
