using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame
{

    public class СhessСlock
    {
        private IEnumerable<IPlayer> players;
        public СhessСlock(IEnumerable<IPlayer> players)
        {
            this.players = players;
        }

        public void Switch()
        {
            var white = players.First(p => p.Color == FigureColor.White);
            var black = players.First(p => p.Color == FigureColor.Black);
            
            if (!white.Timer.TurnOn && !black.Timer.TurnOn)
                white.Timer.Switch();
            else
            {
                black.Timer.Switch();
                white.Timer.Switch();
            }
        }

        public void Stop()
        {
            players.ToList().ForEach(p => p.Timer.Switch(true));
        }

        public bool IsGameOver()
        {
            return players.Any(p => p.Timer.Value == TimeSpan.Zero);
        }
    }
}
