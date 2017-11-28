using System;
using Windows.Storage.Streams;

namespace TCC.ODBDriver.Commands
{
    public class FuelTankLevelInput : Command
    {
        public FuelTankLevelInput(DataReader reader, DataWriter writer) : base(reader, writer)
        {
        }

        public override PID PID => PID.FuelTankLevelInput;

        public override double ParseData(string response, PID pid)
        {
            var fuelTankLevelHexString = response.Substring(4);
            var fuelTankLevel = Convert.ToInt32(fuelTankLevelHexString, 16);
            var result = Convert.ToDouble(fuelTankLevel) * 100 / 255;

            return result;
        }
    }
}
