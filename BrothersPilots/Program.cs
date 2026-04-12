using BrothersPilots.Application.Games;
using BrothersPilots.Hardwares.Boards;
using System.Threading;

namespace BrothersPilots
{
    public class Program
    {
        private const byte SIZE = 4;

        private static Board _board;

        private static ushort _currentValue = 0;

        public static void Main()
        {
            _board = new Board();
            _board.ButtonEvent += Board_ButtonEvent;


            Thread.Sleep(Timeout.Infinite);
        }

        private static void Board_ButtonEvent(ushort value)
        {
            if (_currentValue == value)
            {
                return;
            }

            _currentValue = value;
            var number = Utils.ConvertUint16ToBoolArray(_currentValue);

            if (number > 0)
            {
                Clear();
                Drow(number);
            }
        }

        private static void Drow(byte value)
        {            
            var row = (value - 1) / SIZE;
            var col = (value - 1) % SIZE;

            _board.LcdWrite(row, col, new char[] { (char)219 });
        }

        private static void Clear()
        {
            _board.LcdWrite(0, 0, new char[] { (char)255, (char)255, (char)255, (char)255 });
            _board.LcdWrite(1, 0, new char[] { (char)255, (char)255, (char)255, (char)255 });
            _board.LcdWrite(2, 0, new char[] { (char)255, (char)255, (char)255, (char)255 });
            _board.LcdWrite(3, 0, new char[] { (char)255, (char)255, (char)255, (char)255 });
        }
    }
}
