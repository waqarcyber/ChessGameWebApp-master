using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame
{
    public class ChessCellDto
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public bool IsTarget { get; set; }
        public bool IsMarked { get; set; }
        public FigureDto? Figure { get; set; }
    }
}
