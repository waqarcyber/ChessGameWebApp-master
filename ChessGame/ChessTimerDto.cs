using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame
{
    public class ChessTimerDto
    {
        public PlayerDto[] Players { get; set; }

        public ChessTimerDto()
        {
            Players = new PlayerDto[2];
            Players[0] = new PlayerDto() { Color = FigureColor.White };
            Players[1] = new PlayerDto() { Color = FigureColor.Black };
        }
    }
}