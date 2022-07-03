using BrothersPilots.Hardwares.Boards;
using System;
using System.Diagnostics;
using System.Threading;

namespace BrothersPilots
{
    public class Program
    {
        private static double _battVoltage = 777;

        public static void Main()
        {
            var board = new Board();



            var a = new Timer(x =>
            {
                board.GreenLed.Toggle();
                Debug.WriteLine(board.Buttons.ToString("F3"));
            },
            "green",
            TimeSpan.Zero,
            TimeSpan.FromMilliseconds(500));

            //var b = new Timer(x =>
            //{
            //    board.RedLed.Toggle();
            //    new Thread(new ThreadStart(runner.ThreadProc)).Start();

            //    var btn = board.Buttons;

            //    Console.WriteLine($"{btn}");
            //},
            //"red",
            //TimeSpan.Zero,
            //TimeSpan.FromMilliseconds(100));

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
