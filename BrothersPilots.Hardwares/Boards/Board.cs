using BrothersPilots.Hardwares.Buttons;
using BrothersPilots.Hardwares.Leds;
using System.Device.Gpio;

namespace BrothersPilots.Hardwares.Boards
{
    public class Board
    {
        private readonly GpioController _gpioController = new();
        private readonly ButtonsControllers _buttonsController = new(1, 0x20);
        private readonly StatusLed _statusLed = new();

        public Board()
        {
            GpioPin pin25 = _gpioController.OpenPin(25, PinMode.OutputOpenDrain);

            GreenLed = new(pin25);

            _statusLed.SetStatus(BoardStatus.Ok);
        }

        public Led GreenLed { get; }

        public ushort Buttons => _buttonsController.GetButtosValue();
    }
}
//  var adc1 = new AdcController();
//  var ac0 = adc1.OpenChannel(5);

//var runner = new AdcMesurment((x) =>
//{
//    _battVoltage = x;
//}, ac0);