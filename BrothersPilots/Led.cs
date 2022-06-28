using System.Device.Gpio;

namespace BrothersPilots
{
    internal class Led
    {
        private readonly GpioPin _gpioPin;

        private PinValue _state = PinValue.Low;

        internal Led(GpioPin gpioPin)
        {
            _gpioPin = gpioPin;
        }

        internal PinValue Toggle()
        {
            _state = !(bool)_state;
            _gpioPin.Write(_state);
            return _state;
        }
    }
}
