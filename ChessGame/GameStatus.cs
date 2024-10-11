using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame
{
    public enum GameStatus
    {
        Normal,
        Check,
        Checkmate,
        Stalemate,
        TimeIsUp,
        GiveUp,
        OpponentGiveUp
    }
}
