using ChessGame;
using ChessGameWebApp.Server.Models;
using System.Text.Json;
using Player = ChessGameWebApp.Server.Models.Player;

namespace ChessGameWebApp.Server.Services
{
    internal class GameSessionService : IGameSessionService
    {
        private readonly ILogger<GameSessionService> _logger;
        private readonly List<GameSession> _sessions;
        public GameSessionService(ILogger<GameSessionService> logger, List<GameSession> sessions)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _sessions = sessions ?? throw new ArgumentNullException(nameof(sessions));
        }
        public Task<GameSession> GetSession(int accountId)
        {
            GameSession session;

            lock(_sessions)
                session = _sessions.First(s => s.Players.Any(p => p.Id == accountId));
            _logger.LogInformation($"Get session by {accountId}");

            return Task.FromResult(session);
        }

        public Task<GameSession?> FindSession(int accountId)
        {
            GameSession? session;

            lock (_sessions)
                session = _sessions.FirstOrDefault(s => s.Players.Any(p => p.Id == accountId));
            _logger.LogInformation($"Get session by {accountId}");

            return Task.FromResult(session);
        }

        public Task<bool> CloseSession(int accountId)
        {
            GameSession? session;
            lock (_sessions)
            {
                session = _sessions.FirstOrDefault(s => s.Players.Any(p => p.Id == accountId));
                if (session != null)
                    _sessions.Remove(session);
            }
            _logger.LogInformation($"Remove session by {accountId}");

            return Task.FromResult(true);
        }

        public static GameSession? Create(List<Player> _players, TimeSpan _timer)
        {
            GameSession? session = null;
            List<Player> players = new List<Player>();

            int count = 0;
            lock (_players)
                count = _players.Where(p => p.RivalId == 0).Count();

            if (count > 1)
            {
                lock (_players)
                {
                    if (_players.Count > 1)
                    {
                        var p1 = _players.First(p => p.RivalId == 0);
                        _players.Remove(p1);
                        players.Add(p1);

                        var p2 = _players.First(p => p.RivalId == 0);
                        _players.Remove(p2);
                        players.Add(p2);
                    }
                }
                
                session = new GameSession();
                var board = new Board(true);

                PaintPlayers(players, _timer);
                board.Players = players;
                session.Board = board;
            }
            
            return session;
        }

        public static GameSession? ConcreteCreate(List<Player> _players, TimeSpan _timer)
        {
            GameSession? session = null;
            List<Player> players = new List<Player>();

            int count = 0;
            lock (_players)
                count = _players.Where(p => p.RivalId != 0).Count();

            if (count > 1)
            {
                lock (_players)
                {
                    if (_players.Count > 1)
                    {
                        var p1 = _players.First(p => p.RivalId != 0);
                        _players.Remove(p1);
                        players.Add(p1);

                        var p2 = _players.First(p => p.Id == p1.RivalId);
                        _players.Remove(p2);
                        players.Add(p2);
                    }
                }

                session = new GameSession();
                var board = new Board(true);

                PaintPlayers(players, _timer);
                board.Players = players;
                session.Board = board;
            }

            return session;
        }

        private static void PaintPlayers(List<Player> players, TimeSpan timer)
        {
            if (players.Count == 2)
            {
                players[0].Color = FigureColor.White;
                players[1].Color = FigureColor.Black;

                players.ForEach(p => p.Timer.Delta = timer / 2);
            }
        }
    }
}
