using ChessGame.Figures;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame
{
    public class Board : IEnumerable<Cell>
    {
        public СhessСlock СhessСlock { get; protected set; }
        protected IEnumerable<IPlayer> players;
        public IEnumerable<IPlayer> Players
        {
            get => players; 
            set
            {
                players = value;

                if (value.Count() == 2 
                    && value.FirstOrDefault(p => p.Color == FigureColor.White) != null
                    && value.FirstOrDefault(p => p.Color == FigureColor.Black) != null
                    )
                {
                    players = value;
                    СhessСlock = new СhessСlock(players);
                }
                else
                {
                    throw new ArgumentException("Incorrect players!");
                }
            }
        }
        internal Stack<Figure> MovedFigures { set; get; } = new Stack<Figure>();
        public FigureColor IsAllowedMove { get => Index % 2 == 0 ? FigureColor.White : FigureColor.Black; }
        internal int Index { get; set; } = 0;
        internal Cell[,] Cells;
        public Figure? this[int row, int column]
        {
            get => Cells[row, column].Figure;
            set => Cells[row, column].Figure = value;
        }

        private GameStatus gameStatus;
        public virtual GameStatus GameStatus
        { 
            get => gameStatus;
            set
            {
                gameStatus = value;
            } 
        }

        public IEnumerable<Figure> Figures { get => this.Where(cell => cell.Figure != null).Select(cell => cell.Figure); }

        public Board(bool setup = false)
        {
            Cells = new Cell[8, 8];
            for (int i = 0; i < 8; i++)
                for(int j = 0; j < 8; j++)
                    Cells[i, j] = new Cell(i, j, this);

            if (setup)
                Setup();
        }
        internal Board(Board copy)
        {
            Index = copy.Index;
            Cells = Cells = new Cell[8, 8];

            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    var cell = new Cell(i, j, this);
                    var figure = copy[i, j]?.Clone();

                    if (figure != null)
                        cell.Figure = figure;
                    
                    Cells[i, j] = cell;
                }
        }

        public Cell GetCell(int row, int column)
        {
            return Cells[row, column];
        }
        internal void Setup()
        {
            this[0, 0] = new Rook(FigureColor.Black);
            this[0, 1] = new Knight(FigureColor.Black);
            this[0, 2] = new Bishop(FigureColor.Black);
            this[0, 3] = new Queen(FigureColor.Black);
            this[0, 4] = new King(FigureColor.Black);
            this[0, 5] = new Bishop(FigureColor.Black);
            this[0, 6] = new Knight(FigureColor.Black);
            this[0, 7] = new Rook(FigureColor.Black);
            for (int i = 0; i < 8; i++)
            {
                this[1, i] = new Pawn(FigureColor.Black);
                this[6, i] = new Pawn(FigureColor.White);
            }
            this[7, 0] = new Rook(FigureColor.White);
            this[7, 1] = new Knight(FigureColor.White);
            this[7, 2] = new Bishop(FigureColor.White);
            this[7, 3] = new Queen(FigureColor.White);
            this[7, 4] = new King(FigureColor.White);
            this[7, 5] = new Bishop(FigureColor.White);
            this[7, 6] = new Knight(FigureColor.White);
            this[7, 7] = new Rook(FigureColor.White);
        }

        public void TryMoveBack()
        {
            if (MovedFigures.Count == 0)
                return;

            MoveBack();
            GameStatus = GetGameStatus();
            СhessСlock.Switch();
        }

        internal void MoveBack()
        {
            if (MovedFigures.Count == 0)
                return;

            var last = MovedFigures.Pop();
            int index = last.CheckBoardIndex();

            last.MoveBack();
            
            if (MovedFigures.Count > 0 && index == MovedFigures.Peek().CheckBoardIndex())
                MovedFigures.Pop().MoveBack(true);

            Index--;
        }

        public virtual void TryMove(Cell from, Cell to)
        {
            if (from.Figure?.PossibleMoves.Contains(to) != true)
                throw new InvalidOperationException("Error! Such step is impossible");

            if (GameStatus == GameStatus.TimeIsUp)
                return;

            from.Figure.MoveTo(to);
            GameStatus = GetGameStatus();

            if (GameStatus == GameStatus.Checkmate || GameStatus == GameStatus.Stalemate)
                СhessСlock.Stop();
            else
                СhessСlock.Switch();
        }

        internal void Setup1()
        {
            this[0, 0] = new Rook(FigureColor.Black);
            this[0, 4] = new King(FigureColor.Black);
            this[0, 7] = new Rook(FigureColor.Black);

            this[7, 0] = new Rook(FigureColor.White);

            this[7, 4] = new King(FigureColor.White);
            this[7, 7] = new Rook(FigureColor.White);
        }

        private void Setup2()
        {
            // мат в 2 хода

            this[2, 0] = new Pawn(FigureColor.Black);
            this[2, 2] = new King(FigureColor.Black);
            this[6, 6] = new Queen(FigureColor.White);
            this[7, 1] = new King(FigureColor.White);
            this[3, 0] = new Pawn(FigureColor.White);
            this[4, 3] = new Pawn(FigureColor.White);
            this[6, 7] = new Bishop(FigureColor.White);
            this[3, 3] = new Rook(FigureColor.White);
        }

        internal bool IsCheckToKing(FigureColor color)
        {
            var cell = this.First(cell => cell.Figure is King king && king.Color == color);

            return IsUnderAttack(cell, color == FigureColor.White ? FigureColor.Black : FigureColor.White);
        }

        internal bool IsLastMove(FigureColor color)
        {
            return !this
                .Where(cell => cell.Figure != null)
                .Select(cell => cell.Figure)
                .Where(figure => figure.Color == color)
                .SelectMany(figure => figure.PossibleMoves)
                .Any();
        }

        internal bool IsUnderAttack(Cell cell, FigureColor color)
        {
            return this
                .Where(cell => cell.Figure != null)
                .Select(cell => cell.Figure)
                .Where(figure => figure.Color == color)
                .SelectMany(figure => figure.GetAttackPossibleMoves())
                .Any(move => move == cell);
        }

        public GameStatus GetGameStatus()
        {
            if (СhessСlock.IsGameOver())
                return GameStatus.TimeIsUp;

            FigureColor player = GetCurrentPlayer();

            bool lastMove = IsLastMove(player);
            bool check = IsCheckToKing(player);

            if (check && lastMove)
                return GameStatus.Checkmate;

            if (lastMove)
                return GameStatus.Stalemate;

            if (check)
                return GameStatus.Check;

            return GameStatus.Normal;
        }

        public FigureColor GetCurrentPlayer() => Index % 2 == 0 ? FigureColor.White : FigureColor.Black;
        public IEnumerator<Cell> GetEnumerator() => Cells.Cast<Cell>().GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => Cells.GetEnumerator();
    }
}
