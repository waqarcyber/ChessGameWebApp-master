using ChessGameClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGameClient.AuthWebAPI
{
    public static class AuthExtension
    {
        public static AccountDto Map(this AccountRequestModel account)
        {
            var acc = new AccountDto
            {
                Username = account.Username,
                Login = account.Login,
                Email = account.Email,
                Password = account.Password
            };

            return acc;
        }
    }
}
