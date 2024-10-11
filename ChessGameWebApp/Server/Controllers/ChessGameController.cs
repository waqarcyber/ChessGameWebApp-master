using ChessGameWebApp.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ChessGameWebApp.Server.Models;
using ChessGameClient.Models;

namespace ChessGameWebApp.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChessGameController : Controller
    {
        private readonly ILogger<ChessGameController> _logger;
        private readonly IPlayerService _playerService;
        private readonly IGameSessionService _gameSessionService;

        public ChessGameController(ILogger<ChessGameController> logger, IPlayerService playerService, IGameSessionService gameSessionService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _playerService = playerService ?? throw new ArgumentNullException(nameof(playerService));
            _gameSessionService = gameSessionService ?? throw new ArgumentNullException(nameof(gameSessionService));
        }
        [HttpGet("GetUser")]
        [Authorize]
        public Task<UserInfo> GetUser()
        {
            
            try
            {
                var siteUser = new UserInfo();
                siteUser.UserName = User.Claims.First(c => c.Type == ClaimTypes.Email).Value;
                siteUser.AccountId = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
                siteUser.Roles = User.Claims
                    .Where(claim => claim.Type == ClaimTypes.Role)
                    .Select(c => c.Value)
                    .ToList();

                return Task.FromResult(siteUser);
            }
            catch
            {
                return Task.FromResult(new UserInfo());
            }
        }

        
        [HttpGet ("AddOrRemovePlayer")]
        [Authorize]
        public async Task<bool> AddOrRemovePlayer(int rivalId)
        {
            int id = GetUserId();
            Player player = new Player() { Id = id, RivalId = rivalId };
            return await _playerService.AddOrRemove(player);
        }

        
        [HttpGet("PlayerCount")]
        [Authorize]
        public async Task<int> PlayerCount()
        {
            return await _playerService.Count();
        }

        [HttpGet("SessionExists")]
        [Authorize]
        public async Task<bool> SessionExists()
        {
            int accountId = GetUserId();
            var result = await _gameSessionService.FindSession(accountId);
            return result == null ? false : true;
        }

        private int GetUserId()
        {
            return int.Parse(User.Claims.FirstOrDefault(c =>  c.Type == ClaimTypes.NameIdentifier)?.Value);
        }
    }
}