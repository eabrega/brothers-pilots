using BrothersPilots.Hardwares.Boards;
using System;
using System.Threading;

namespace BrothersPilots
{
    public class Program
    {
        public static void Main()
        {
            var board = new Board();

            //var a = new Timer(x =>
            //{
            //    board.GreenLed.Toggle();
            //},
            //"green",
            //TimeSpan.Zero,
            //TimeSpan.FromMilliseconds(25));

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
