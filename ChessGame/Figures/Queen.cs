using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Figures
{
    public class Queen : Figure
    {
        public Queen(FigureColor color) : base(color)
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

                list.AddRange(Position.GetCellsInDirection(Directions.Up));
                list.AddRange(Position.GetCellsInDirection(Directions.Right));
                list.AddRange(Position.GetCellsInDirection(Directions.Left));
                list.AddRange(Position.GetCellsInDirection(Directions.Down));
            }

            return list;
        }
        internal override Figure Clone() => new Queen(Color);
    }
}
