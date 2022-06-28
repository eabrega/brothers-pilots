using System.Device.Adc;

namespace BrothersPilots.Hardwares.Adcs
{
    public delegate void ExampleCallback(double lineCount);

    public class AdcMesurment
    {
        private static int counter = 0;
        private const uint VOLTAGES_ARRAY_LENGHT = 5;
        private readonly ExampleCallback _callback;
        private readonly AdcChannel _adcChannel;
        private double[] voltages;

        public AdcMesurment(
            ExampleCallback callback,
            AdcChannel adcChannel)
        {
            _callback = callback;
            _adcChannel = adcChannel;
            voltages = new double[VOLTAGES_ARRAY_LENGHT];
        }

        public void ThreadProc()
        {
            var value = _adcChannel.ReadValue();
            var voltage = value / 4095d * 3.43d;                 

            if (counter >= VOLTAGES_ARRAY_LENGHT)
            {
                _callback?.Invoke(AverageVoltage());
                voltages = new double[VOLTAGES_ARRAY_LENGHT];
                counter = 0;
            }

            voltages[counter] = voltage;
            counter++;
        }

        private double AverageVoltage()
        {
            var accum = 0d;

            foreach (var item in voltages)
            {
                accum += item;
            }

            return accum / voltages.Length;
        }
    }
}
