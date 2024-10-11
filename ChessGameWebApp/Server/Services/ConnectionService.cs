using ChessGameWebApp.Server.Models;

namespace ChessGameWebApp.Server.Services
{
    public class ConnectionService : IConnectionService
    {
        private readonly ILogger<ConnectionService> _logger;
        private readonly List<Connection> _connections;
        public ConnectionService(ILogger<ConnectionService> logger, List<Connection> connections)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _connections = connections ?? throw new ArgumentNullException(nameof(connections));
        }

        public Task Add(int accountId, string connectionId)
        {
            var connection = new Connection
            {
                AccountId = accountId,
                ConnectionId = connectionId
            };

            lock(_connections)
                _connections.Add(connection);

            _logger.LogInformation($"add conntection {accountId} - {connectionId}");
            return Task.CompletedTask;
        }

        public Task Remove(string connectionId)
        {
            lock(_connections)
            {
                var connection = _connections.First(x => x.ConnectionId == connectionId);
                _connections.Remove(connection);

                _logger.LogInformation($"remove conntection {connectionId}");
            }

            return Task.CompletedTask;
        }

        public Task<IReadOnlyList<string>> GetConnections(int[] accountIds)
        {
            IEnumerable<Connection> connections;
            lock(_connections)
                connections = _connections.Where(con => accountIds.Contains(con.AccountId));
            IReadOnlyList<string> res = connections.Select(con => con.ConnectionId).ToList();

            return Task.FromResult(res);
        }
    }
}
