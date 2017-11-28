using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage.Streams;

namespace TCC.ODBDriver.Commands
{
    public abstract class Command : ICommand
    {
        private readonly DataReader _reader;
        private readonly DataWriter _writer;

        public abstract double ParseData(string response, PID pid);
        public abstract PID PID { get; }

        public Command(DataReader reader, DataWriter writer)
        {
            _reader = reader;
            _writer = writer;
        }

        public async Task<double> GetValue()
        {
            var result = 0.0;
            try
            {
                result = await RetrieveData(Mode.CurrentData, PID);
            }
            catch (Exception)
            {
                //Ignored
            }

            return result;
        }

        private static string GetCommand(Mode mode, PID pid)
        {
            return $"{Convert.ToUInt32(mode):X2}{Convert.ToUInt32(pid):X2}\r";
        }

        private static string NormalizeResponse(string res, string delimiter)
        {
            var result = res.Replace(" ", "").Replace(">", "");
            var resSplitted = result.Split('\r');
            var expectedRes = resSplitted.FirstOrDefault(x => x.Contains(delimiter));

            if (expectedRes != null)
                return expectedRes;

            return res.Replace("\r", "").Replace(" ", "");
        }
        
        protected async Task<double> RetrieveData(Mode mode, PID pid)
        {
            var result = 0.0;
            const int retries = 2;
            try
            {
                var cmd = GetCommand(mode, pid);
                var response = await SendCommand(cmd);
                await Task.Delay(50);
                var pidString = pid.ToString("X").Replace("000000", "");

                for (var i = 0; i < retries; i++)
                {
                    if (/*!response.Contains("41") && */!response.Contains(pidString))
                    {
                        response = await SendCommand(cmd);
                        await Task.Delay(50);
                    }
                    else
                    {
                        break;
                    }
                }

                if (response.Contains(pidString))
                {
                    response = NormalizeResponse(response, $"41{pidString}");
                    result = ParseData(response, pid);
                }
            }
            catch (Exception)
            {
                //nothing
            }

            return result;
        }

        private async Task<string> SendCommand(string command)
        {
            var result = "Failure sending command!";
            try
            {
                _writer.WriteString(command);
                await _writer.StoreAsync();
                await _writer.FlushAsync();

                IAsyncOperation<uint> count = _reader.LoadAsync(512);
                count.AsTask().Wait();

                result = _reader.ReadString(count.GetResults());
            }
            catch (Exception e)
            {
                var x = e;
            }

            return result;
        }
    }
}
