using BrothersPilots.Applications;
using BrothersPilots.Hardwares;
using BrothersPilots.Hardwares.Adcs;
using System;
using System.Device.Adc;
using System.Diagnostics;
using System.Threading;

namespace BrothersPilots
{
    public class Program
    {
        private static double _battVoltage = 0;

        public static void Main()
        {
            var aww = new Game(222);
            var board = new Board();

            var adc1 = new AdcController();
            var ac0 = adc1.OpenChannel(5);

            var runner = new AdcMesurment((x) =>
            {
                _battVoltage = x;
            }, ac0);

            var a = new Timer(x =>
            {
                board.GreenLed.Toggle();
                Debug.WriteLine(_battVoltage.ToString("F3"));
            },
            "green",
            TimeSpan.Zero,
            TimeSpan.FromMilliseconds(500));

            var b = new Timer(x =>
            {
                board.RedLed.Toggle();
                new Thread(new ThreadStart(runner.ThreadProc)).Start();

                var btn = board.Buttons;

                Console.WriteLine($"{btn}");
            },
            "red",
            TimeSpan.Zero,
            TimeSpan.FromMilliseconds(100));

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
