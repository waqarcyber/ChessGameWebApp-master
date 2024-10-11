using ChessGameWebApp.Server.Models;

namespace ChessGameWebApp.Server.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly List<Player> _players;

        public PlayerService(List<Player> players)
        {
            _players = players ?? throw new ArgumentNullException(nameof(players));
        }

        public Task<bool> AddOrRemove(Player player)
        {
            bool addPlayer = false;
            lock (_players)
            {
                var p = _players.FirstOrDefault(p => p.Id == player.Id);

                if (p != null)
                    _players.Remove(p);
                else
                {
                    _players.Add(player);
                    addPlayer = true;
                }
            }

            return Task.FromResult(addPlayer);
        }

        public Task Add(Player player)
        {
            lock (_players)
            {
                if (!_players.Any(p => p.Id == player.Id))
                    _players.Add(player);
            }

            return Task.CompletedTask;
        }

        public Task Remove(Player player)
        {
            lock (_players)
            {
                if (_players.Contains(player))
                    _players.Remove(player);
            }
            return Task.CompletedTask;
        }

        public Task Remove(int id)
        {
            lock (_players)
            {
                var p = _players.FirstOrDefault(p => p.Id == id);

                if (p != null)
                    _players.Remove(p);
            }
            return Task.CompletedTask;
        }

        public Task<int> Count()
        {
            lock(_players)
            {
                return Task.FromResult(_players.Count);
            }
        }
    }
}
