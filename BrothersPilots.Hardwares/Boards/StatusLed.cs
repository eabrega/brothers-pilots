﻿using Iot.Device.Ws28xx.Esp32;
using System.Drawing;

namespace BrothersPilots.Hardwares.Boards
{
    public class StatusLed
    {
        private static readonly Color _okLedColor = Color.FromArgb(0xF, 0, 120, 0);
        private static readonly Color _worningLedColor = Color.FromArgb(0xF, 140, 60, 0);
        private static readonly Color _errorLedColor = Color.FromArgb(0xF, 120, 0, 0);
        private static readonly Sk6812 _statusLed = new(27, 1, 1);

        public void SetStatus(BoardStatus newStatus)
        {
            switch (newStatus)
            {
                case BoardStatus.Ok:
                    _statusLed?.Image?.SetPixel(0, 0, _okLedColor);
                    break;
                case BoardStatus.Warning:
                    _statusLed?.Image?.SetPixel(0, 0, _worningLedColor);
                    break;
                case BoardStatus.Error:
                    _statusLed?.Image?.SetPixel(0, 0, _errorLedColor);
                    break;
            }
            _statusLed.Update();
        }
    }

    public enum BoardStatus
    {
        Ok,
        Warning,
        Error,
    }
}
