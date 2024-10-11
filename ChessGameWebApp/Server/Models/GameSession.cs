using ChessGame;

namespace ChessGameWebApp.Server.Models
{
    public class GameSession
    {
        public IEnumerable<Player> Players {
            get
            {
                return (IEnumerable <Player>)Board.Players;
            }
        }
        public Board Board { get; set; }

        public bool IsAllowedMove(int accountId)
        {
            var player = Players.First(p => p.Id == accountId);

            return player.Color == Board.IsAllowedMove;
        }

        public Player GetPlayer(int accountId)
        {
            return Players.First(p => p.Id == accountId);
        }
    }
}
