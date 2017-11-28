using System;
using Windows.Storage.Streams;

namespace TCC.ODBDriver.Commands
{
    public class ThrottlePosition : Command
    {
        public ThrottlePosition(DataReader reader, DataWriter writer) : base(reader, writer)
        {
        }

        public override PID PID => PID.ThrottlePosition;

        public override double ParseData(string response, PID pid)
        {
            var throttlePositionHexString = response.Substring(4);
            var throttlePosition = Convert.ToInt32(throttlePositionHexString, 16);
            var result = Convert.ToDouble(throttlePosition) * 100 / 255;

            return result;
        }
    }
}
