using ChessGame.Figures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame
{
    internal static class AI
    {
        private static float GetValue(this Figure figure)
        {

            switch (figure)
            {
                case Pawn _:
                    return 10f;
                case Bishop _:
                case Knight _:
                    return 30f;
                case Rook _:
                    return 50f;
                case Queen _:
                    return 90f;
                case King _:
                    return 900f;
                default:
                    throw new NotImplementedException($"Error type {figure.GetType().Name}");
            }
        }

        private static float GetPositionValue(this Figure figure)
        {
            switch (figure)
            {
                case Pawn _:
                    return figure.Color == FigureColor.White
                        ? PawnEvalWhite[figure.Position.Row, figure.Position.Column]
                        : -PawnEvalBlack[figure.Position.Row, figure.Position.Column];
                case Bishop _:
                    return figure.Color == FigureColor.White
                        ? BishopEvalWhite[figure.Position.Row, figure.Position.Column]
                        : -BishopEvalBlack[figure.Position.Row, figure.Position.Column];
                case Knight _:
                    return figure.Color == FigureColor.White
                        ? KnightEval[figure.Position.Row, figure.Position.Column]
                        : -KnightEval[figure.Position.Row, figure.Position.Column];
                case Rook _:
                    return figure.Color == FigureColor.White
                        ? RookEvalWhite[figure.Position.Row, figure.Position.Column]
                        : -RookEvalBlack[figure.Position.Row, figure.Position.Column];
                case Queen _:
                    return figure.Color == FigureColor.White
                        ? QueenEval[figure.Position.Row, figure.Position.Column]
                        : -QueenEval[figure.Position.Row, figure.Position.Column];
                case King _:
                    return figure.Color == FigureColor.White
                        ? KingEvalWhite[figure.Position.Row, figure.Position.Column]
                        : -KingEvalBlack[figure.Position.Row, figure.Position.Column];
                default:
                    throw new NotImplementedException($"Error type {figure.GetType().Name}");
            }
        }
        
        internal static float GetWeight(this Figure figure)
        {
            return (figure.Color == FigureColor.White ? 1.0f : -1.0f) * figure.GetValue() + figure.GetPositionValue();
        }

        private static float GetBoardEvaluation(this Board board)
        {

            float sum = 0;

            foreach (var cell in board.Cells)
            {
                if (cell.Figure != null)
                {
                    sum += cell.Figure.GetWeight();
                }
            }

            if (board.GameStatus == GameStatus.Checkmate)
            {
                if (board.IsAllowedMove == FigureColor.White)
                    sum += 9999f;
                else
                    sum -= 9999f;
            }

            return sum;
        }

        public static Dictionary<Figure, Cell> GetMoveEvaluation(this Board board, int depth = 1)
        {
            Dictionary<Figure, Cell> dic = new Dictionary<Figure, Cell>();
            Figure figure = null;
            Cell move = null;

            float res = 0, score;
            var color = board.GetCurrentPlayer() == FigureColor.White;

            if (color)
                res = -9999f;
            else
                res = 9999f;
             
            foreach (var f in board.Figures)
            {
                var moves = f.PossibleMoves;
                
                foreach (var m in moves)
                {
                    f.MoveTo(m);

                    score = board.GetBoardEvaluation();
                    if (color && res < score)
                    {
                        res = score;
                        figure = f;
                        move = m;
                    }
                    else
                    if (!color && res > score)
                    {
                        res = score;
                        figure = f;
                        move = m;
                    }

                    board.MoveBack();
                }
            }

            dic.Add(figure, move);

            return dic;
        }

        private static float[,] Reverce(float[,] array)
        {
            float[,] mass = (float[,])array.Clone();

            for (int i = 0; i < array.GetLength(0) / 2; i++)
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    float temp = mass[i, j];
                    mass[i, j] = mass[array.GetLength(0) - 1 - i, array.GetLength(1) - 1 - j];
                    mass[array.GetLength(0) - 1 - i, array.GetLength(1) - 1 - j] = temp;
                }

            return mass;
        }

        private readonly static float[,] PawnEvalWhite = new float[8, 8]
        {
            {0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f},
            {5.0f, 5.0f, 5.0f, 5.0f, 5.0f, 5.0f, 5.0f, 5.0f},
            {1.0f, 1.0f, 2.0f, 3.0f, 3.0f, 2.0f, 1.0f, 1.0f},
            {0.5f, 0.5f, 1.0f, 2.5f, 2.5f, 1.0f, 0.5f, 0.5f},
            {0.0f, 0.0f, 0.0f, 2.5f, 3.5f, 0.0f, 0.0f, 0.0f},
            {0.5f, -0.5f, -1.0f, 0.0f, 0.0f, -1.0f, -0.5f, 0.5f},
            {0.5f, 1.0f, 1.0f, -2.0f, -2.0f, 1.0f, 1.0f, 0.5f},
            {0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f}
        };

        private readonly static float[,] PawnEvalBlack = Reverce(PawnEvalWhite);

        private readonly static float[,] KnightEval = new float[8, 8]
        {
            {-5.0f, -4.0f, -3.0f, -3.0f, -3.0f, -3.0f, -4.0f, -5.0f},
            {-4.0f, -2.0f,  0.0f,  0.0f,  0.0f,  0.0f, -2.0f, -4.0f},
            {-3.0f,  0.0f,  0.5f,  1.5f,  1.5f,  0.5f,  0.0f, -3.0f},
            {-3.0f,  0.5f,  1.5f,  2.0f,  2.0f,  1.5f,  0.5f, -3.0f},
            {-3.0f,  0.0f,  1.5f,  2.0f,  2.0f,  1.5f,  0.0f, -3.0f},
            {-3.0f,  0.5f,  0.5f,  1.5f,  1.5f,  0.5f,  0.5f, -3.0f},
            {-4.0f, -2.0f,  0.0f,  0.5f,  0.5f,  0.0f, -2.0f, -4.0f},
            {-5.0f, -4.0f, -3.0f, -3.0f, -3.0f, -3.0f, -4.0f, -5.0f}
        };

        private readonly static float[,] BishopEvalWhite = new float[8, 8]
        {
            { -2.0f, -1.0f, -1.0f, -1.0f, -1.0f, -1.0f, -1.0f, -2.0f},
            { -1.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f, -1.0f},
            { -1.0f,  0.0f,  0.5f,  1.0f,  1.0f,  0.5f,  0.0f, -1.0f},
            { -1.0f,  0.5f,  0.5f,  1.0f,  1.0f,  0.5f,  0.5f, -1.0f},
            { -1.0f,  0.0f,  1.0f,  1.0f,  1.0f,  1.0f,  0.0f, -1.0f},
            { -1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  1.0f, -1.0f},
            { -1.0f,  0.5f,  0.0f,  0.0f,  0.0f,  0.0f,  0.5f, -1.0f},
            { -2.0f, -1.0f, -1.0f, -1.0f, -1.0f, -1.0f, -1.0f, -2.0f}
        };

        private readonly static float[,] BishopEvalBlack = Reverce(BishopEvalWhite);

        private readonly static float[,] RookEvalWhite = {
            {  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f},
            {  0.5f,  1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  0.5f},
            { -0.5f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f, -0.5f},
            { -0.5f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f, -0.5f},
            { -0.5f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f, -0.5f},
            { -0.5f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f, -0.5f},
            { -0.5f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f, -0.5f},
            {  0.0f,   0.0f, 0.0f,  0.5f,  0.5f,  0.0f,  0.0f,  0.0f}
        };

        private readonly static float[,] RookEvalBlack = Reverce(RookEvalWhite);

        private readonly static float[,] QueenEval = {
            { -2.0f, -1.0f, -1.0f, -0.5f, -0.5f, -1.0f, -1.0f, -2.0f},
            { -1.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f, -1.0f},
            { -1.0f,  0.0f,  0.5f,  0.5f,  0.5f,  0.5f,  0.0f, -1.0f},
            { -0.5f,  0.0f,  0.5f,  0.5f,  0.5f,  0.5f,  0.0f, -0.5f},
            {  0.0f,  0.0f,  0.5f,  0.5f,  0.5f,  0.5f,  0.0f, -0.5f},
            { -1.0f,  0.5f,  0.5f,  0.5f,  0.5f,  0.5f,  0.0f, -1.0f},
            { -1.0f,  0.0f,  0.5f,  0.0f,  0.0f,  0.0f,  0.0f, -1.0f},
            { -2.0f, -1.0f, -1.0f, -0.5f, -0.5f, -1.0f, -1.0f, -2.0f}
        };

        private readonly static float[,] KingEvalWhite = {
            { -3.0f, -4.0f, -4.0f, -5.0f, -5.0f, -4.0f, -4.0f, -3.0f},
            { -3.0f, -4.0f, -4.0f, -5.0f, -5.0f, -4.0f, -4.0f, -3.0f},
            { -3.0f, -4.0f, -4.0f, -5.0f, -5.0f, -4.0f, -4.0f, -3.0f},
            { -3.0f, -4.0f, -4.0f, -5.0f, -5.0f, -4.0f, -4.0f, -3.0f},
            { -2.0f, -3.0f, -3.0f, -4.0f, -4.0f, -3.0f, -3.0f, -2.0f},
            { -1.0f, -2.0f, -2.0f, -2.0f, -2.0f, -2.0f, -2.0f, -1.0f},
            {  2.0f,  2.0f,  0.0f,  0.0f,  0.0f,  0.0f,  2.0f,  2.0f },
            {  2.0f,  3.0f,  1.0f,  0.0f,  0.0f,  1.0f,  3.0f,  2.0f }
        };

        private readonly static float[,] KingEvalBlack = Reverce(KingEvalWhite);
    }
}
