using AuthService.Services;
using ChessGameClient;
using ChessGameClient.AuthWebAPI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Security.Authentication;
using System.Security.Claims;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IRegistrationService _registrationService;
        private readonly ILogger<AuthController> _logger;
        public AuthController(IRegistrationService registrationService, ILogger<AuthController> logger)
        {
            _registrationService = registrationService ?? throw new ArgumentNullException(nameof(registrationService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        [HttpPost("Registaration")]
        public async Task<IActionResult> Registration([FromBody]AccountRequestModel account)
        {
            try
            {
                var acc = new Account()
                {
                    Username = account.Username,
                    Login = account.Login,
                    Email = account.Email,
                    Password = account.Password
                };
                await _registrationService.AddAccount(acc);
                return Ok("Registration is successful!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Authentication")]
        public async Task<IActionResult> Authentication(string login, string password)
        {
            try
            {
                var result = await _registrationService.GetTokens(login, password);

                return Ok(result);
            }
            catch (AuthenticationException ex)
            {
                _logger.LogWarning(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                return BadRequest("Unknown error!");
            }
        }

        [HttpGet("AutoAuth")]
        public async Task<IActionResult> AutoAuthentication(string refreshToken)
        {
            try
            {
                var result = await _registrationService.GetTokens(refreshToken);

                return Ok(result);
            }
            catch (AuthenticationException ex)
            {
                _logger.LogWarning(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                return BadRequest("Unknown error!");
            }
        }

        [HttpGet("LogOut")]
        [Authorize]
        public async Task LogOut()
        {
            var accountId = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            await _registrationService.LogOut(accountId);
        }
    }
}
