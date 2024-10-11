using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame
{        
    /// <summary>
    /// Класс для клиет-серверного взаимодействия
    /// </summary>
    public class ChessCell : Cell, IChessObservable
    {
        private bool _isPointer;
        private bool _isMarked;
        private string? _figureName;
        private bool _isMarkedHelp;

        public bool IsPointer { get => _isPointer; set { _isPointer = value; ((IChessObservable)this).Notify(); } }
        public bool IsMarked { get => _isMarked; set { _isMarked = value; ((IChessObservable)this).Notify(); } }
        public string? FigureName { get => _figureName; set { _figureName = value; ((IChessObservable)this).Notify(); } }
        public bool IsHelp { get => _isMarkedHelp; set { _isMarkedHelp = value; ((IChessObservable)this).Notify(); } }
        public List<IChessObserver> Observers { get; set; } = new List<IChessObserver>();

        public ChessCell(int row, int column, Board board) : base(row, column, board)
        {
        }

        private string? GetActualFigureName()
        {
            if (Figure == null)
                return null;

            return $"{Figure.Color}{Figure.GetType().Name}";
        }

        public override Figure? Figure
        {
            get { return base.Figure; }

            set { 
                base.Figure = value;
                FigureName = GetActualFigureName();
            }
        }

        internal new ChessBoard Board => (ChessBoard)base.Board;
        public async Task Click()
        {
            await Board.Click(Row, Column);
        }
    }
}
