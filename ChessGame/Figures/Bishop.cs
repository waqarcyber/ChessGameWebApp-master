using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Figures
{
    public class Bishop : Figure
    {
        public Bishop(FigureColor color) : base(color)
        {
        }
        internal override List<Cell> GetAllPossibleMoves()
        {
            var list = new List<Cell>();
            if (Position != null)
            {
                list.AddRange(Position.GetCellsInDirection(Directions.LeftUp));
                list.AddRange(Position.GetCellsInDirection(Directions.RightDown));
                list.AddRange(Position.GetCellsInDirection(Directions.LeftDown));
                list.AddRange(Position.GetCellsInDirection(Directions.RightUp));
            }

            return list;
        }
        internal override Figure Clone() => new Bishop(Color);
    }
}
