using System;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace TCC.ODBDriver.Commands
{
    public class EngineTemperature : Command
    {
        public EngineTemperature(DataReader reader, DataWriter writer) : base(reader, writer)
        {
        }

        public override PID PID => PID.EngineTemperature;


        public override double ParseData(string response, PID pid)
        {
            var engineTemperatureHexString = response.Substring(4);
            var engineTemperature = Convert.ToInt32(engineTemperatureHexString, 16);
            var result = Convert.ToDouble(engineTemperature) - 40;

            return result;
        }
    }
}
