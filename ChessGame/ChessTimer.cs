using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame
{
    public class ChessTimer
    {
        private bool turnOn = false;
        public bool TurnOn {
            set
            {
                turnOn = value;

                if (turnOn)
                    endTime = DateTime.UtcNow + delta;
                else
                    delta = endTime - DateTime.UtcNow;
                    
            }

            get => turnOn;
        }
        public void Switch(bool off = false)
        {
            if (off)
                TurnOn = false;
            else
                TurnOn = !TurnOn;
        }
        public TimeSpan Value 
        { 
            get => turnOn ? (endTime - DateTime.UtcNow < TimeSpan.Zero ? TimeSpan.Zero : endTime - DateTime.UtcNow) : Delta;
        }
        private TimeSpan delta = TimeSpan.FromMinutes(1);
        public TimeSpan Delta
        {
            get => delta < TimeSpan.Zero ? TimeSpan.Zero : delta;
            set => delta = value;
        }
        private DateTime endTime;
        public DateTime EndTime
        {
            get => endTime;
            set => endTime = value;
        }
    }
}
