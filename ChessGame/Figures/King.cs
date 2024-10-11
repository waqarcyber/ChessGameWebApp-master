using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Figures
{
    public class King : Figure
    {
        public King(FigureColor color) : base(color)
        {
        }
        internal override List<Cell> GetAllPossibleMoves()
        {
            var moves = new List<Cell>();
            if (Position != null)
            {
                AddUsualMoves(moves);
            }
            return moves;
        }

        protected override IEnumerable<Cell> GetPossibleMovesAreNotAnderAttack()
        {
            var moves = GetAllPossibleMoves();
            
            if (IsFirstMove == 0)
                AddCastlingMoves(moves);

            return moves
                .Where(m => !Board.IsUnderAttack(m, Color == FigureColor.White ? FigureColor.Black : FigureColor.White))
                .ToList();
        }

        private void AddUsualMoves(List<Cell> moves)
        {
            for (int i = Position.Row - 1; i <= Position.Row + 1; i++)
                for (int j = Position.Column - 1; j <= Position.Column + 1; j++)
                    if (i >= 0 && j >= 0 && i < 8 && j < 8 && !(i == Position.Row && j == Position.Column))
                        moves.Add(Board.Cells[i, j]);
        }

        private void AddCastlingMoves(List<Cell> moves)
        {
            var left = Position.GetCellsInDirection(Directions.Left);
            var right = Position.GetCellsInDirection(Directions.Right);

            if (left.Count != 0)
                if (left.Last()?.Figure is Rook rook && rook.IsFirstMove == 0
                    && !Board.IsUnderAttack(Board.Cells[Position.Row, Position.Column - 1], Color == FigureColor.White ? FigureColor.Black : FigureColor.White))
                    moves.Add(Board.Cells[Position.Row, Position.Column - 2]);

            if (right.Count != 0)
                if (right.Last()?.Figure is Rook rook && rook.IsFirstMove == 0
                    && !Board.IsUnderAttack(Board.Cells[Position.Row, Position.Column + 1], Color == FigureColor.White ? FigureColor.Black : FigureColor.White))
                    moves.Add(Board.Cells[Position.Row, Position.Column + 2]);
        }

        internal override void MoveTo(Cell to, bool doubleMove = false)
        {
            if (IsFirstMove == 0)
                Сastling(to);

            base.MoveTo(to);
        }

        private void Сastling(Cell cell)
        {
            if (Position.Column - cell.Column == -2)
            {
                Position.GetCellsInDirection(Directions.Right)
                    .Last().Figure?
                    .MoveTo(Board.Cells[Position.Row, Position.Column + 1], true);
            }
            else
            if (Position.Column - cell.Column == 2)
            {
                Position.GetCellsInDirection(Directions.Left)
                    .Last().Figure?
                    .MoveTo(Board.Cells[Position.Row, Position.Column - 1], true);
            }
        }
        internal override Figure Clone() => new King(Color);
    }
}
