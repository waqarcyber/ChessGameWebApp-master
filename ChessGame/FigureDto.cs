using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame
{
    public class FigureDto
    {
        public FigureColor Color { get; set; }
        public int IsFirstMove { get; set; }
        public string Type { get; set; }
    }
}
