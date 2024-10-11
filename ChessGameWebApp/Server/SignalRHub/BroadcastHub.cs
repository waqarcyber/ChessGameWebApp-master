using ChessGameClient.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace ChessGameWebApp.Server.SignalRHub
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BroadcastHub : Hub
    {

        private readonly ILogger<GameHub> _logger;
        public BroadcastHub(ILogger<GameHub> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task SendMessage(string message)
        {
            var answer = new ChatMessage()
            {
                Message = message,
                Username = GetUserName(Context),
                AccountId = GetCurrentAccountId(Context),
                Time = DateTime.Now
            };
            await Clients.All.SendAsync("ReceiveMessage", answer);
        }

        private string GetUserName(HubCallerContext context)
        {
            return context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName).Value;
        }

        private int GetCurrentAccountId(HubCallerContext context)
        {
            try
            {
                return int.Parse(context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            }
            catch
            {
                return -1;
            }
        }
    }
}
