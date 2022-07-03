using BrothersPilots.Hardwares.Adcs;
using BrothersPilots.Hardwares.Buttons;
using BrothersPilots.Hardwares.Leds;
using System;
using System.Device.Adc;
using System.Device.Gpio;
using System.Diagnostics;
using System.Threading;
using nanoFramework.Hardware.Esp32;

namespace BrothersPilots.Hardwares.Boards
{
    public class Board
    {
        private double _powerVoltage = 0;
        private readonly AdcController _adc1 = new();
        private readonly AdcMesurment _adcMesurment;
        private readonly GpioController _gpioController = new();
        private readonly ButtonsControllers _buttonsController = new(1, 0x20);
        private readonly StatusLed _statusLed = new();
        private readonly Timer _timer;

        public Board()
        {
            var pin25 = _gpioController.OpenPin(25, PinMode.OutputOpenDrain);
            GreenLed = new(pin25);

            Configuration.SetPinFunction(26, DeviceFunction.ADC1_CH0);
            _adc1.ChannelMode = AdcChannelMode.SingleEnded;
            var ac0 = _adc1.OpenChannel(0);
            _adcMesurment = new AdcMesurment((x) => MasurmentPowerVoltage(x), ac0);
            _timer = new Timer(x => MainTask(), "run", TimeSpan.Zero, TimeSpan.FromMilliseconds(100));

            _statusLed.SetStatus(BoardStatus.Ok);
        }

        public Led GreenLed { get; }

        public ushort Buttons => _buttonsController.GetButtosValue();

        private void MasurmentPowerVoltage(double value)
        {
            _powerVoltage = value;
        }

        private void MainTask()
        {
            new Thread(new ThreadStart(_adcMesurment.ThreadProc)).Start();
            Debug.WriteLine(_powerVoltage.ToString());
            GreenLed.Toggle();
            CheckPowerVoltage();
        }

        private void CheckPowerVoltage()
        {
            switch (_powerVoltage)
            {
                case > 2:
                    _statusLed.SetStatus(BoardStatus.Ok);
                    break;
                case > 0.9:
                    _statusLed.SetStatus(BoardStatus.Warning);
                    break;
                default:
                    _statusLed.SetStatus(BoardStatus.Error);
                    break;
            }
        }
    }
}
