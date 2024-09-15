using Iot.Device.Mcp23xxx;
using nanoFramework.Hardware.Esp32;
using System.Device.I2c;

namespace BrothersPilots.Hardwares.Buttons
{
    public class ButtonsControllers
    {
        private readonly Mcp23017 _controllerA;
        private readonly Mcp23017 _controllerB;

        public ButtonsControllers()
        {
            Configuration.SetPinFunction(22, DeviceFunction.I2C1_CLOCK);
            Configuration.SetPinFunction(21, DeviceFunction.I2C1_DATA);

            var connectionSettings = new I2cConnectionSettings(1, 0x21, I2cBusSpeed.FastMode);
            var i2cDeviceA = I2cDevice.Create(connectionSettings);

            _controllerA = new Mcp23017(i2cDeviceA);

            _controllerA.WriteByte(Register.IODIR, 0b11111111, Port.PortA);
            _controllerA.WriteByte(Register.GPIO, 0b00000000, Port.PortA);
            _controllerA.WriteByte(Register.IPOL, 0b11111111, Port.PortA);
            _controllerA.WriteByte(Register.IODIR, 0b00000000, Port.PortB);
            _controllerA.WriteByte(Register.GPIO, 0b00000000, Port.PortB);
            _controllerA.WriteByte(Register.GPPU, 0b11111111, Port.PortA);

            var connectionSettingsB = new I2cConnectionSettings(1, 0x20, I2cBusSpeed.FastMode);
            var i2cDeviceB = I2cDevice.Create(connectionSettingsB);

            _controllerB = new Mcp23017(i2cDeviceB);

            _controllerB.WriteByte(Register.IODIR, 0b11111111, Port.PortA);
            _controllerB.WriteByte(Register.GPIO, 0b00000000, Port.PortA);
            _controllerB.WriteByte(Register.IPOL, 0b11111111, Port.PortA);
            _controllerB.WriteByte(Register.IODIR, 0b00000000, Port.PortB);
            _controllerB.WriteByte(Register.GPIO, 0b00000000, Port.PortB);
            _controllerB.WriteByte(Register.GPPU, 0b11111111, Port.PortA);
        }

        public ushort GetButtosValue()
        {
            var valueA = _controllerA.ReadByte(Register.GPIO, Port.PortA);
            var valueB = _controllerB.ReadByte(Register.GPIO, Port.PortA);
            var high = (ushort)(valueB << 8);

            return (ushort)(high + valueA);
        }

        public void SetValue(ushort value)
        {
            byte upper = (byte)(value >> 8);
            byte lower = (byte)(value & 0xff);

            _controllerA.WriteByte(Register.GPIO, upper, Port.PortB);
            _controllerB.WriteByte(Register.GPIO, lower, Port.PortB);
        }
    }
}
