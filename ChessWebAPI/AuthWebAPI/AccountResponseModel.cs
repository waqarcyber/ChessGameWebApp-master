using ChessGameClient.Models;
using System;
using System.Text.Json.Serialization;

namespace ChessGameClient.AuthWebAPI
{
    [Serializable]
    public class AccountResponseModel
    {
        public AccountDto Account { get; set; }
        public string? Token { get; set; }

        public AccountResponseModel(AccountDto account, string? token = null)
        {
            Account = account;
            Token = token;
        }
    }
}