using BrothersPilots.Application.Games;
using BrothersPilots.Hardwares.Boards;
using System.Threading;

namespace BrothersPilots
{
    public class Program
    {
        private const byte SIZE = 4;
        private static Board _board;
        private static GameBoard _gameBoard;
        private static ushort _currentValue = 0;

        public static void Main()
        {
            _board = new Board();
            _gameBoard = new GameBoard();
            _board.ButtonEvent += Board_ButtonEvent;
            Clear();
            Drow(_gameBoard.GetLeds());
            Thread.Sleep(Timeout.Infinite);
        }

        private static void Board_ButtonEvent(ushort value)
        {
            var number = Utils.ConvertUint16ToBoolArray(value);
            if (number > 0)
            {
                Clear();
                _gameBoard.TogleButton(Col(number), Row(number));
                Drow(_gameBoard.GetLeds());
            }
        }

        private static void Drow(bool[] values)
        {
            for (ushort i = 1; i < values.Length + 1; i++)
            {
                var col = Col(i);
                var row = Row(i);
                _board.LcdWrite(row, col, new char[] { values[i - 1] ? (char)219 : (char)255 });
            }
        }

        private static void Clear()
        {
            _board.LcdWrite(0, 0, new char[] { (char)255, (char)255, (char)255, (char)255 });
            _board.LcdWrite(1, 0, new char[] { (char)255, (char)255, (char)255, (char)255 });
            _board.LcdWrite(2, 0, new char[] { (char)255, (char)255, (char)255, (char)255 });
            _board.LcdWrite(3, 0, new char[] { (char)255, (char)255, (char)255, (char)255 });
        }

        private static int Row(ushort value) => (value - 1) / SIZE;

        private static int Col(ushort value) => (value - 1) % SIZE;
    }
}
