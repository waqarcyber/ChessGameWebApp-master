using AuthWebAPI.Services.Impl;
using JwtToken;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models;
using Repositories;
using System.Security.Authentication;

namespace AuthService.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly ILogger<RegistrationService> _logger;
        private readonly IUnitOfWork _uow;
        private readonly IPasswordHasher<Account> _passwordHasher;
        private readonly IAccessTokenService _accessTokenService;
        private readonly IRefreshTokenService _refreshTokenService;

        public RegistrationService(
            ILogger<RegistrationService> logger,
            IUnitOfWork uow,
            IPasswordHasher<Account> passwordHasher,
            IAccessTokenService accessTokenService,
            IRefreshTokenService refreshTokenService
            )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
            _accessTokenService = accessTokenService ?? throw new ArgumentNullException(nameof(accessTokenService));
            _refreshTokenService = refreshTokenService ?? throw new ArgumentNullException(nameof(refreshTokenService));
        }

        public async Task AddAccount(Account account)
        {
            try
            {
                account.Password = _passwordHasher.HashPassword(account, account.Password);
                var role = await _uow.RoleRepository.FindByName("user");
                account.Roles.Add(role);
                await _uow.AccountRepository.Add(account);
                await _uow.SaveChangesAsync();
                _logger.LogInformation($"Account {account.Email} has been created!");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogWarning(ex.Message);
                throw new Exception("Login or e-mail already exists!");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                throw new Exception("Unknown error!");
            }
        }

        public Task<IReadOnlyList<Account>> GetAccounts()
        {
            _logger.LogInformation(nameof(GetAccounts));
            return _uow.AccountRepository.GetAll();
        }

        public Task<List<Account>> SearchAccounts(string username)
        {
            _logger.LogInformation(nameof(GetAccounts));
            return _uow.AccountRepository.SearchByUsername(username);
        }

        public Task GetAccountById(int id)
        {
            _logger.LogInformation(nameof(GetAccountById));
            return _uow.AccountRepository.GetById(id);
        }

        public async Task BanAccount(Account account)
        {
            _logger.LogInformation($"BanAccount {account.Login}");
            account.IsBanned = true;
            await _uow.AccountRepository.Update(account);
            await _uow.SaveChangesAsync();
        }

        public async Task UnBanAccount(Account account)
        {
            _logger.LogInformation($"UnBanAccount {account.Login}");
            account.IsBanned = false;
            await _uow.AccountRepository.Update(account);
            await _uow.SaveChangesAsync();
        }

        public Task<Account?> GetAccountByEmail(string email)
        {
            _logger.LogInformation(nameof(GetAccountByEmail));
            return _uow.AccountRepository.FindByEmail(email);
        }

        public Task<Account?> GetAccountByLogin(string login)
        {
            _logger.LogInformation(nameof(GetAccountByLogin));
            return _uow.AccountRepository.FindByLogin(login);
        }

        private Task<Account?> GetAccountByToken(string token)
        {
            _logger.LogInformation(nameof(GetAccountByLogin));
            return _uow.AccountRepository.FindByToken(token);
        }

        private async Task SaveToken(string refreshToken, Account account)
        {
            await _uow.RefreshTokenRepository.Add(new RefreshToken() { Token = refreshToken, Account = account });
            await _uow.SaveChangesAsync();
        }

        public async Task<JwtTokens?> GetTokens(string login, string password)
        {
            _logger.LogInformation(nameof(GetTokens));
            var account = await GetAccountByLogin(login) ?? await GetAccountByEmail(login);

            var isCorrect = account != null
                && _passwordHasher.VerifyHashedPassword(account, account.Password, password) != PasswordVerificationResult.Failed;

            if (!isCorrect)
                throw new AuthenticationException("Login or password are not correct!");

            if (account.IsBanned)
                throw new AuthenticationException("Account is banned!");

            var accessToken = _accessTokenService.GenerateToken(account);
            var refreshToken = _refreshTokenService.GenerateToken(account);

            await SaveToken(refreshToken, account);

            return new JwtTokens() { AccessToken = accessToken, RefreshToken = refreshToken };
        }

        public async Task<JwtTokens?> GetTokens(string refreshToken)
        {
            _logger.LogInformation(nameof(GetTokens));

            var account = await GetAccountByToken(refreshToken);

            if (account == null)
                throw new AuthenticationException("Invalid token!");

            var accessToken = _accessTokenService.GenerateToken(account);

            if (JwtToken.TokenService.TokenIsExpired(refreshToken))
            {
                refreshToken = _refreshTokenService.GenerateToken(account);
                await SaveToken(refreshToken, account);
            }

            return new JwtTokens() { AccessToken = accessToken, RefreshToken = refreshToken };
        }

        public async Task LogOut(int accountId)
        {
            var token = await _uow.RefreshTokenRepository.FindByAccountId(accountId);

            if (token != null)
            {
                await _uow.RefreshTokenRepository.Remove(token);
                await _uow.SaveChangesAsync();
            }
        }
    }
}
