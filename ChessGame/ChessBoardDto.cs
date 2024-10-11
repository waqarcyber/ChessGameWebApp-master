using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame
{
    public class ChessBoardDto
    {
        public int Index { get; set; }
        public List<ChessCellDto> Cells { get; set; } = new List<ChessCellDto>();
    }
}
