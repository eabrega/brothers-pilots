using System;

namespace BrothersPilots.Application.Games
{
    public class Utils
    {
        public static byte ConvertUint16ToBoolArray(ushort value)
        {
            // Проверяем, установлен ли ровно один бит
            // (value & (value - 1)) сбрасывает младший установленный бит.
            // Если после этого остаётся 0, значит изначально был ровно один бит.
            if (value == 0 || (value & (value - 1)) != 0)
            {
                return 0; // 0 бит или несколько бит
            }

            // Ищем позицию установленного бита (1-based, чтобы 0 означало "ошибка")
            byte position = 1;
            while ((value & 1) == 0)
            {
                value >>= 1;
                position++;
            }
            return position;
        }
    }
}
