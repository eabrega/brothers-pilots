using BrothersPilots.Hardwares.Boards;
using System.Threading;

namespace BrothersPilots
{
    public class Program
    {
        public static void Main()
        {
            var board = new Board();

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
