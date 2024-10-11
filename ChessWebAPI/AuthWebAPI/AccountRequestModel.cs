using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGameClient.AuthWebAPI
{
    public class AccountRequestModel
    {
        public string Email { get; set; } = string.Empty;

        public string Login { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string Username { get; set; } = string.Empty;

        public AccountRequestModel() { }

        public AccountRequestModel(string login, string password, string email, string username)
        {
            Login = login;
            Password = password;
            Email = email;
            Username = username;
        }
    }
}
