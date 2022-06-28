using System.Device.Gpio;

namespace BrothersPilots.Hardwares.Leds
{
    public class Led
    {
        private readonly GpioPin _gpioPin;

        private PinValue _state = PinValue.Low;

        public Led(GpioPin gpioPin)
        {
            _gpioPin = gpioPin;
        }

        public PinValue Toggle()
        {
            _state = !(bool)_state;
            _gpioPin.Write(_state);
            return _state;
        }
    }
}
