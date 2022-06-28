using System;
using System.Device.Gpio;
using System.Device.I2c;
using System.Device.Adc;
using Iot.Device.Mcp23xxx;
using nanoFramework.Hardware.Esp32;
using System.Diagnostics;
using System.Threading;

namespace BrothersPilots
{
    public class Program
    {
        private static GpioController _GpioController;
        private static double _battVoltage = 0;

        public static void Main()
        {
            _GpioController = new GpioController();
            GpioPin pin4 = _GpioController.OpenPin(4, PinMode.OutputOpenDrain);
            GpioPin pin0 = _GpioController.OpenPin(0, PinMode.OutputOpenDrain);

            Configuration.SetPinFunction(7, DeviceFunction.I2C1_CLOCK);
            Configuration.SetPinFunction(8, DeviceFunction.I2C1_DATA);

            var ab = Configuration.GetFunctionPin(DeviceFunction.I2C1_CLOCK);
            var ba = Configuration.GetFunctionPin(DeviceFunction.I2C1_DATA);

            var connectionSettings = new I2cConnectionSettings(1, 0x20);

            var i2cDevice = I2cDevice.Create(connectionSettings);
            var ICH = new Mcp23017(i2cDevice);

            var connectionSettings2 = new I2cConnectionSettings(1, 0x21);

            var i2cDevice2 = I2cDevice.Create(connectionSettings2);
            var ICL = new Mcp23017(i2cDevice2);

            ICH.WriteByte(Register.IODIR, 0, Port.PortB);
            ICL.WriteByte(Register.IODIR, 0, Port.PortB);

            var ledGreen = new Led(pin4);
            var ledRed = new Led(pin0);

            var adc1 = new AdcController();
            var ac0 = adc1.OpenChannel(5);

            var runner = new AdcMesurment((x) =>
            {
                _battVoltage = x;
            }, ac0);

            var a = new Timer(x =>
            {
                ledGreen.Toggle();
                // Debug.WriteLine(_battVoltage.ToString("F3"));
            },
            "green",
            TimeSpan.Zero,
            TimeSpan.FromMilliseconds(500));

            var b = new Timer(x =>
            {
                var state = ledRed.Toggle();
                new Thread(new ThreadStart(runner.ThreadProc)).Start();

                var dH = ICH.ReadByte(Register.GPIO, Port.PortB);
                var dL = ICL.ReadByte(Register.GPIO, Port.PortB);

                var dd = ICH.ReadUInt16(Register.GPIO);

                Console.WriteLine($"{dH},{dL},{dd}");
            },
            "red",
            TimeSpan.Zero,
            TimeSpan.FromMilliseconds(100));

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
