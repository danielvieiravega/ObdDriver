using System;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace TCC.ODBDriver.Commands
{
    public class Speed : Command
    {
        public Speed(DataReader reader, DataWriter writer) 
            : base(reader, writer)
        {
        }

        public override PID PID => PID.Speed;
        

        public override double ParseData(string response, PID pid)
        {
            var speedHexString = response.Substring(4);
            var speed = Convert.ToInt32(speedHexString, 16);
            var result = Convert.ToDouble(speed);

            return result;
        }
    }
}
