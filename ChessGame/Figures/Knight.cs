using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Figures
{
    public class Knight : Figure
    {
        public Knight(FigureColor color) : base(color)
        {
        }
        internal override List<Cell> GetAllPossibleMoves()
        {
            var list = new List<Cell>();
            if (Position != null)
            {
                for (int i = -2; i <= 2; i++)
                    for (int j = -2; j <= 2; j++)
                    {
                        if (Position.Row + i >= 0 && Position.Row + i < 8 && Position.Column + j >= 0 && Position.Column + j < 8)
                            if (i != j && i != -j && i != 0 && j != 0)
                                list.Add(Board.Cells[Position.Row + i, Position.Column + j]);
                    }
            }

            return list;
        }
        internal override Figure Clone() => new Knight(Color);
    }
}
