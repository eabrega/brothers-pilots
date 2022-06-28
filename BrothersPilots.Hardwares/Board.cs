using BrothersPilots.Hardwares.Leds;
using BrothersPilots.Hardwares.Buttons;
using BrothersPilots.Hardwares.Adcs;
using System.Device.Gpio;

namespace BrothersPilots.Hardwares
{
    public class Board
    {
        private static GpioController _GpioController;
        private readonly ButtonsControllers _buttonsController;

        public Board()
        {
            _GpioController = new GpioController();
            GpioPin pin4 = _GpioController.OpenPin(4, PinMode.OutputOpenDrain);
            GpioPin pin0 = _GpioController.OpenPin(0, PinMode.OutputOpenDrain);

            GreenLed = new Led(pin4);
            RedLed = new Led(pin0);

            _buttonsController = new ButtonsControllers(1, 0x20);
        }

        public Led GreenLed { get; }

        public Led RedLed { get; }

        public ushort Buttons => _buttonsController.GetButtosValue();
    }
}
