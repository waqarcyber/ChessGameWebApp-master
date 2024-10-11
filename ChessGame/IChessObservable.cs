using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame
{
    public interface IChessObservable
    {
        List<IChessObserver> Observers { get; set; }
        void Notify()
        {
            Observers.ForEach(o => o.UpdateAsync());
        }

        void Subscribe(IChessObserver observer)
        {
            Observers.Add(observer);
        }

        void Remove(IChessObserver observer)
        {
            Observers.Remove(observer);
        }
    }
}
