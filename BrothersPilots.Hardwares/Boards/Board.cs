using BrothersPilots.Hardwares.Adcs;
using BrothersPilots.Hardwares.Buttons;
using BrothersPilots.Hardwares.Leds;
using BrothersPilots.Hardwares.Tools;
using Iot.Device.CharacterLcd;
using System;
using System.Device.Adc;
using System.Device.Gpio;
using System.Device.I2c;
using System.Diagnostics;
using System.Threading;

namespace BrothersPilots.Hardwares.Boards
{
    public class Board
    {
        private double _powerVoltage = 0;
        private readonly AdcController _adc1 = new();
        private readonly AdcMesurment _adcMesurment;
        private readonly GpioController _gpioController = new();
        private readonly ButtonsControllers _buttonsController = new();
        private readonly StatusLed _statusLed = new();
        private readonly Timer _timer;
        private readonly Timer _timer2;
        private readonly Timer _timer3;

        private readonly GpioPin t1;
        private readonly GpioPin t2;

        private readonly I2cDevice _i2cLcdDevice;
        private readonly LcdInterface _lcdInterface;
        private readonly Hd44780 _lcd;
        private long sum = 0;
        private int value = 0;

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

            _i2cLcdDevice = I2cDevice.Create(new I2cConnectionSettings(busId: 1, deviceAddress: 0x25, I2cBusSpeed.FastMode));
            _lcdInterface = LcdInterface.CreateI2c(_i2cLcdDevice, false);
            _lcd = new Lcd2004(_lcdInterface);

            _timer = new Timer(x => MainTask(), "run", TimeSpan.Zero, TimeSpan.FromMilliseconds(200));
            _timer2 = new Timer(x => CheckButtons(), "run", TimeSpan.Zero, TimeSpan.FromMilliseconds(50));
            _timer3 = new Timer(x => ToggleLamp(), "run", TimeSpan.Zero, TimeSpan.FromMilliseconds(500));

            _statusLed.SetStatus(BoardStatus.Ok);
            //I2cScanner.Run();
        }

        public Led GreenLed { get; }

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

        private void CheckButtons()
        {
            sum++;

            _lcd.UnderlineCursorVisible = false;
            _lcd.SetCursorPosition(0, 0);
            _lcd.Write(_buttonsController.GetButtosValue().ToString() + "      ");
            _lcd.SetCursorPosition(0, 1);
            _lcd.Write(sum.ToString());
            _lcd.SetCursorPosition(17, 0);
            _lcd.Write("v3");
        }

        private void ToggleLamp()
        {
            value <<= 1;
            value++;
            if (value > ushort.MaxValue)
            {
                value = 0;
            }
            
            _buttonsController.SetValue((ushort)value);
            _lcd.SetCursorPosition(0, 2);
            _lcd.Write(value.ToString() + "     ");          
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
