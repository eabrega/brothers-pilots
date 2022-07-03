using Iot.Device.Mcp23xxx;
using nanoFramework.Hardware.Esp32;
using System.Device.I2c;

namespace BrothersPilots.Hardwares.Buttons
{
    public class ButtonsControllers
    {
        private readonly Mcp23017 _controller;

        public ButtonsControllers(int deviceId, int address)
        {
            Configuration.SetPinFunction(22, DeviceFunction.I2C1_CLOCK);
            Configuration.SetPinFunction(21, DeviceFunction.I2C1_DATA);

            var connectionSettings = new I2cConnectionSettings(deviceId, address, I2cBusSpeed.FastMode);
            var i2cDevice = I2cDevice.Create(connectionSettings);

            _controller = new Mcp23017(i2cDevice);
            _controller.WriteByte(Register.IODIR, 0, Port.PortB);
        }

        public ushort GetButtosValue()
        {
            return _controller.ReadUInt16(Register.GPIO);
        }
    }
}
