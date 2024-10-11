using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Figures
{
    public class Rook : Figure
    {
        public Rook(FigureColor color) : base(color)
        {
        }
        internal override List<Cell> GetAllPossibleMoves()
        {
            var list = new List<Cell>();
            if (Position != null)
            {
                list.AddRange(Position.GetCellsInDirection(Directions.Up));
                list.AddRange(Position.GetCellsInDirection(Directions.Right));
                list.AddRange(Position.GetCellsInDirection(Directions.Left));
                list.AddRange(Position.GetCellsInDirection(Directions.Down));
            }

            return list;
        }
        internal override Figure Clone() => new Rook(Color);
    }
}
