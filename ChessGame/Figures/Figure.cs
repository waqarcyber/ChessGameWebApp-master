using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame
{
    public abstract class Figure
    {
        internal Stack<SavedMove> SavedMoves { set; get; } = new Stack<SavedMove>();
        internal int MovesCount { get; set; }
        public FigureColor Color { get; private set; }
        public Cell Position { get; set; }
        internal abstract List<Cell> GetAllPossibleMoves();
        public IEnumerable<Cell> PossibleMoves
        {
            get => GetPossibleMoves();
        }
        public int IsFirstMove { get; set; }
        internal Figure(FigureColor color, int firstMove = 0)
        {
            Color = color;
            IsFirstMove = firstMove;
        }
        internal Board Board { get => Position.Board; }

        internal abstract Figure Clone();

        internal virtual void MoveTo(Cell cell, bool doubleMove = false)
        {
            cell.Figure?.SaveMoves(cell);
            SaveMoves(Position);

            Position.Figure = null;
            cell.Figure = this;

            if (!doubleMove)
            {
                IsFirstMove++;
                Board.Index++;
            }
        }
        internal int CheckBoardIndex()
        {
            return SavedMoves.Peek().BoardIndex;
        }

        internal void MoveBack(bool doubleMove = false)
        {
            var savedMove = SavedMoves.Pop();
            Position.Figure = null;
            savedMove.Move.Figure = this;

            if (!doubleMove)
                IsFirstMove--;
        }

        protected void SaveMoves(Cell lastMove)
        {
            Board.MovedFigures.Push(this);
            SavedMoves.Push(new SavedMove()
            {
                Move = lastMove,
                BoardIndex = Board.Index
            });
        }
        public bool IsMove()
        {
            return Color == FigureColor.White && Board.Index % 2 == 0 || Color == FigureColor.Black && Board.Index % 2 == 1;
        }
        protected virtual IEnumerable<Cell> GetPossibleMoves()
        {
            if (!IsMove())
                return new List<Cell>();

            var moves = GetPossibleMovesAreNotAnderAttack()
                .Where(i => i.Figure?.Color != Color);

            return GetPossibleMovesWithoutCheckToKing(moves);
        }

        internal virtual IEnumerable<Cell> GetAttackPossibleMoves() => GetAllPossibleMoves();

        internal IEnumerable<Cell> GetPossibleMovesWithoutCheckToKing(IEnumerable<Cell> moves)
        {
            var testBoard = new Board(Board);
            var figure = testBoard[Position.Row, Position.Column];
            var correctCells = new List<Cell>();

            foreach (var move in moves)
            {
                var to = testBoard.Cells[move.Row, move.Column];
                figure.MoveTo(to);
                if (!testBoard.IsCheckToKing(figure.Color))
                    correctCells.Add(move);

                testBoard.MoveBack();
            }

            return correctCells;
        }

        protected virtual IEnumerable<Cell> GetPossibleMovesAreNotAnderAttack()
        {
            return GetAllPossibleMoves();
        }

        public override string ToString()
        {
            return GetType().Name;
        }

        internal virtual Type Type() => GetType();
    }
}
