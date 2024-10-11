using Repositories;

namespace AuthService.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IUnitOfWork _uow;

        public UserService(
            ILogger<UserService> logger,
            IUnitOfWork uow
            )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        }

    }
}
