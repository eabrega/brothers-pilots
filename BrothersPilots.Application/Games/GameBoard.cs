using System;
using System.Diagnostics;

namespace BrothersPilots.Application.Games
{
    public class GameBoard
    {
        private const byte SIZE = 4;
        private const byte BOARD_SIZE = (SIZE * SIZE);
        private readonly bool[] _leds = new bool[BOARD_SIZE];

        public GameBoard()
        {
            var rnd = new Random(BOARD_SIZE);

            for (int i = 0; i < BOARD_SIZE; i++)
            {
                _leds[i] = i == rnd.Next(BOARD_SIZE - 1);
            }
        }

        public void TogleButton(int col, int row)
        {
            var index = (row * SIZE) + col;
            //_leds[index] = !_leds[index];

            //reverce col
            for (int i = 0, colIndex = col; i < SIZE; i++)
            {
               // if (i != row)
               // {
                    _leds[colIndex] = !_leds[colIndex];
              //  }

                colIndex += SIZE;
            }

            ////reverce row
            for (int i = 0, rowIndex = (row * SIZE); i < SIZE; i++)
            {
                //if (i != col)
                //{
                    _leds[rowIndex] = !_leds[rowIndex];
                // }
                rowIndex++;
            }
        }

        public bool[] GetLeds() => _leds;
    }
}
