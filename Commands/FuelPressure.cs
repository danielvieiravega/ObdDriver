using System;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace TCC.ODBDriver.Commands
{
    public class FuelPressure : Command
    {
        public FuelPressure(DataReader reader, DataWriter writer) : base(reader, writer)
        {
        }

        public override PID PID => PID.FuelPressure;
        

        public override double ParseData(string response, PID pid)
        {
            var fuelPressureHexString = response.Substring(4);
            var fuelPressure = Convert.ToInt32(fuelPressureHexString, 16);
            var result = Convert.ToDouble(fuelPressure) * 3;

            return result;
        }
    }
}
