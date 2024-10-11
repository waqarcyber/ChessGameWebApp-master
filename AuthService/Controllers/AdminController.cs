using AuthService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize (Roles = "admin")]
    public class AdminController : ControllerBase
    {
        private readonly IRegistrationService _registrationService;
        private readonly ILogger<AuthController> _logger;
        public AdminController(IRegistrationService registrationService, ILogger<AuthController> logger)
        {
            _registrationService = registrationService ?? throw new ArgumentNullException(nameof(registrationService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("Search")]
        public async Task<IActionResult> Search(string username)
        {
            try
            {
                List<Account> result = await _registrationService.SearchAccounts(username);

                return Ok(result.Select(i => Map(i)));

            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                return BadRequest("Unknown error!");
            }
        }

        [HttpGet("Ban")]
        public async Task<IActionResult> BanOrUnban(string email)
        {
            try
            {
                var account = await _registrationService.GetAccountByEmail(email);

                if (account != null)
                {
                    if (account.IsBanned)
                        await _registrationService.UnBanAccount(account);
                    else
                        await _registrationService.BanAccount(account);
                }

                return Ok();

            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                return BadRequest("Unknown error!");
            }
        }

        private AccountDto Map(Account account)
        {
            return new AccountDto()
            {
                Id = account.Id,
                Login = account.Login,
                Email = account.Email,
                IsBanned = account.IsBanned,
                Username = account.Username,
                Roles = account.Roles.Select(x => x.Name).ToList()
            };
        }
    }
}
