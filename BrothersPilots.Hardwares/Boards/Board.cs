using BrothersPilots.Hardwares.Adcs;
using BrothersPilots.Hardwares.Buttons;
using BrothersPilots.Hardwares.Leds;
using System;
using System.Device.Adc;
using System.Device.Gpio;
using System.Threading;

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
        private readonly Timer _timer2;
        private bool state = false;

        private readonly GpioPin t1;
        private readonly GpioPin t2;


        public Board()
        {
            var pin25 = _gpioController.OpenPin(25, PinMode.OutputOpenDrain);
            GreenLed = new(pin25);

            t1 = _gpioController.OpenPin(21, PinMode.Output);
            t2 = _gpioController.OpenPin(22, PinMode.Output);

            t1.Write(PinValue.High);
            t2.Write(PinValue.High);

            _adc1.ChannelMode = AdcChannelMode.SingleEnded;
            var ac0 = _adc1.OpenChannel(0);
            _adcMesurment = new AdcMesurment((x) => MasurmentPowerVoltage(x), ac0);
            _timer = new Timer(x => MainTask(), "run", TimeSpan.Zero, TimeSpan.FromMilliseconds(200));
            _timer2 = new Timer(x => Tick(), "run", TimeSpan.Zero, TimeSpan.FromMilliseconds(50));

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
            Console.WriteLine(_powerVoltage.ToString());
            GreenLed.Toggle();
            CheckPowerVoltage();
        }

        public void Tick()
        {
            state = !state;
            t1.Write(state);
            t2.Write(!state);
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
