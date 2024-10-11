using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Figures
{
    public class Pawn : Figure
    {
        private int passentIndex = 0;
        public Pawn(FigureColor color) : base(color)
        {
        }
        internal override void MoveTo(Cell cell, bool doubleMove = false)
        {

            if (cell.Row > 0 && Board[cell.Row - 1, cell.Column] is Pawn pawn && pawn.IsFirstMove == 1 && pawn.Color == FigureColor.White && pawn.passentIndex == Board.Index)
            {
                pawn.SaveMoves(pawn.Position);
                pawn.Position.Figure = null;
            }
            if (cell.Row < 7 && Board[cell.Row + 1, cell.Column] is Pawn p && p.IsFirstMove == 1 && p.Color == FigureColor.Black && p.passentIndex == Board.Index)
            {
                p.SaveMoves(p.Position);
                p.Position.Figure = null;
            }

            base.MoveTo(cell, doubleMove);
            
            if (IsFirstMove == 1 && (Position.Row == 3 || Position.Row == 4))
                passentIndex = Board.Index;

            if (Position.Row == 0 || Position.Row == 7)
                Transformation(new Queen(Color));
        }

        internal override List<Cell> GetAllPossibleMoves()
        {
            var moves = new List<Cell>();

            if (Position != null)
            {
                var direction = Color == FigureColor.White ? Directions.Up : Directions.Down;

                moves.AddRange(AddAttackFields(direction));
                moves.AddRange(AddForwardFields(direction));
                moves.AddRange(Passent(direction));
            }

            return moves;
        }

        internal override IEnumerable<Cell> GetAttackPossibleMoves()
        {
            var moves = new List<Cell>();

            if (Position != null)
            {
                var direction = Color == FigureColor.White ? Directions.Up : Directions.Down;

                moves.AddRange(AddAttackFields(direction));
            }

            return moves;
        }

        private List<Cell> AddAttackFields(Directions direction)
        {
            var attackFields = new List<Cell>();
            if (direction == Directions.Up)
            {
                attackFields.AddRange(Position.GetCellsInDirection(Directions.LeftUp, 1));
                attackFields.AddRange(Position.GetCellsInDirection(Directions.RightUp, 1));
            }
            else
            {
                attackFields.AddRange(Position.GetCellsInDirection(Directions.LeftDown, 1));
                attackFields.AddRange(Position.GetCellsInDirection(Directions.RightDown, 1));
            }

            return attackFields.Where(cell => cell.Figure != null && cell.Figure.Color != Color).ToList();
        }

        private List<Cell> AddForwardFields(Directions direction)
        {
            var moves = new List<Cell>();
            int range = IsFirstMove == 0 && (Position.Row == 1 || Position.Row == 6) ? 2 : 1;
            moves.AddRange(Position.GetCellsInDirection(direction, range).Where(i => i.Figure == null));

            return moves;
        }

        private List<Cell> Passent(Directions direction)
        {
            var moves = new List<Cell>();
            if (Position.Row == 3 || Position.Row == 4)
            {
                if (Position.Column > 0 && Board[Position.Row, Position.Column - 1] is Pawn pawn && pawn.IsFirstMove == 1 && pawn.Color != Color && pawn.passentIndex == Board.Index)
                {
                    if (direction == Directions.Up)
                    {
                        if (Board[Position.Row - 1, Position.Column - 1] == null)
                            moves.Add(Board.Cells[Position.Row - 1, Position.Column - 1]);
                    }
                    else
                    {
                        if (Board[Position.Row + 1, Position.Column - 1] == null)
                            moves.Add(Board.Cells[Position.Row + 1, Position.Column - 1]);
                    }
                }

                if (Position.Column < 7 && Board[Position.Row, Position.Column + 1] is Pawn p && p.IsFirstMove == 1 && p.Color != Color && p.passentIndex == Board.Index)
                {
                    if (direction == Directions.Up)
                    {
                        if (Board[Position.Row - 1, Position.Column + 1] == null)
                            moves.Add(Board.Cells[Position.Row - 1, Position.Column + 1]);
                    }
                    else
                    {
                        if (Board[Position.Row + 1, Position.Column + 1] == null)
                            moves.Add(Board.Cells[Position.Row + 1, Position.Column + 1]);
                    }
                }
            }

            return moves;
        }

        private void Transformation(Figure figure)
        {
            Position.Figure = figure;
        }
        internal override Figure Clone() => new Pawn(Color);
    }
}
