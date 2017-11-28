using System;
using Windows.Storage.Streams;

namespace TCC.ODBDriver.Commands
{
    public class IntakeAirTemperature : Command
    {
        public IntakeAirTemperature(DataReader reader, DataWriter writer) : base(reader, writer)
        {
        }

        public override PID PID => PID.IntakeAirTemperature;

        public override double ParseData(string response, PID pid)
        {
            var intakeAirTemperatureHexString = response.Substring(4);
            var intakeAirTemperature = Convert.ToInt32(intakeAirTemperatureHexString, 16);
            var result = Convert.ToDouble(intakeAirTemperature) - 40;

            return result;
        }
    }
}
