using ChessGame.Figures;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame
{
    public class ChessBoard : Board, IEnumerable<ChessCell>, IChessObservable
    {
        public List<IChessObserver> Observers { get; set; } = new List<IChessObserver>();
        public IPlayer? Player { get; private set; }

        public delegate Task<bool> CheckMove(Cell from, Cell to);
        private CheckMove CheckFigureMove { get; set; } = delegate { return Task.FromResult(true); };
        private ChessCell target;
        internal ChessCell Target
        {
            get => target;
            set
            {
                if (target != null)
                    target.IsPointer = false;
                target = value;
                target.IsPointer = true;
            }
        }
        public override GameStatus GameStatus
        {
            get
            {
                return base.GameStatus;
            }
            set
            {
                base.GameStatus = value;
                ((IChessObservable)this).Notify();
            }
        }

        public string GameStatusDescription
        {
            get
            {
                if (Player == null)
                    return string.Empty;

                switch (GameStatus)
                {
                    case GameStatus.Normal:
                        return GetCurrentPlayer() == Player.Color ? "Ваш ход" : "Ход соперника";
                    case GameStatus.Check:
                        return GetCurrentPlayer() == Player.Color ? "Соперник объявил вам ШАХ!" : "Вы объявили ШАХ сопернику!";
                    case GameStatus.Stalemate:
                        return "Пат, Ничья!";
                    case GameStatus.Checkmate:
                        return GetCurrentPlayer() == Player.Color ? "Соперник объявил вам МАТ!" : "Вы объявили МАТ сопернику!";
                    case GameStatus.TimeIsUp:
                        return GetCurrentPlayer() == Player.Color ? "Ваше время вышло!" : "Время соперника вышло!";
                    case GameStatus.GiveUp:
                        return "Вы сдались!";
                    case GameStatus.OpponentGiveUp:
                        return "Ваш соперник сдался!";
                    default:
                        return "Unknown status";
                }
            }
        }

        public void UpdateGameStatus()
        {
            GameStatus = GetGameStatus();
        }

        public void GetHelp()
        {
            var res = this.GetMoveEvaluation().First();

            GetCell(res.Key.Position.Row, res.Key.Position.Column).IsHelp = true;
            GetCell(res.Value.Row, res.Value.Column).IsHelp = true;
        }

        private void ClearHelp()
        {
            foreach (ChessCell cell in Cells)
                cell.IsHelp = false;
        }

        public ChessBoard(bool setup = false, bool onSameBoard = false)
        {
            Cells = new ChessCell[8, 8];
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    Cells[i, j] = new ChessCell(i, j, this);

            Players = CreatePlayers();

            if (!onSameBoard)
                Player = Players.First(p => p.Color == FigureColor.White);

            if (setup)
                Setup();
        }

        private List<Player> CreatePlayers()
        {
            var players = new List<Player>
            {
                new Player() { Color = FigureColor.White },
                new Player() { Color = FigureColor.Black }
            };

            return players;
        }

        public void SetCurrentPlayer(FigureColor playerColor)
        {
            Player = Players.First(p => p.Color == playerColor);
            ((IChessObservable)this).Notify();
        }

        public new ChessCell GetCell(int row, int column)
        {
            return (ChessCell)Cells[row, column];
        }
        private void ClearPossibleMoves()
        {
            foreach (ChessCell cell in Cells)
                cell.IsMarked = false;
        }

        private void ShowPossibleMoves(IEnumerable<Cell>? cells)
        {
            ClearPossibleMoves();

            if (cells != null)
            foreach (Cell cell in cells)
                GetCell(cell.Row, cell.Column).IsMarked = true;
        }
        public async Task Click(int row, int column)
        {
            var currentCell = (ChessCell)Cells[row, column];

            if (currentCell.IsMarked)
            {
                if (await CheckFigureMove(target, currentCell))
                {
                    if (Player == null || Player?.Color == IsAllowedMove)
                        TryMove(Target, currentCell);
                }
                
                ClearPossibleMoves();
            }
            else
            {
                ShowPossibleMoves(currentCell.Figure?.PossibleMoves);
            }

            Target = currentCell;
        }

        public override void TryMove(Cell from, Cell to)
        {
            base.TryMove(from, to);
            ClearHelp();
            ClearPossibleMoves();
        }

        public void SetCheckMethod(CheckMove checkMove)
        {
            CheckFigureMove = checkMove;
        }

        public new IEnumerator<ChessCell> GetEnumerator() => Cells.Cast<ChessCell>().GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => Cells.GetEnumerator();
    }
}
