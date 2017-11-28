using System;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using TCC.ODBDriver.Commands;

namespace TCC.ODBDriver
{
    public class ObdDriver
    {
        private DataReader _reader;
        private DataWriter _writer;
        private StreamSocket _streamSocket;
        private RfcommDeviceService _deviceService;

        public async Task<bool> InitializeConnection()
        {
            DeviceInformationCollection _deviceCollection;
            DeviceInformation _selectedDevice;

            var index = 0;

            var result = false;
            var device = RfcommDeviceService.GetDeviceSelector(RfcommServiceId.SerialPort);
            _deviceCollection = await DeviceInformation.FindAllAsync(device);

            if (_deviceCollection.Count > 0)
            {
                _selectedDevice = _deviceCollection[index];
                if (_selectedDevice != null)
                    _deviceService = await RfcommDeviceService.FromIdAsync(_selectedDevice.Id);

                if (_deviceService != null)
                {
                    try
                    {
                        _streamSocket = new StreamSocket();

                        await _streamSocket.ConnectAsync(_deviceService.ConnectionHostName,
                            _deviceService.ConnectionServiceName);

                        SetupStreams();

                        await SendInitializationCommands();

                        result = true;
                    }
                    catch (Exception e)
                    {
                        var xx = e;
                    }

                }
            }

            return result;
        }

        private void SetupStreams()
        {
            _reader = new DataReader(_streamSocket.InputStream)
            {
                InputStreamOptions = InputStreamOptions.Partial
            };
            _writer = new DataWriter(_streamSocket.OutputStream);
        }

        /// <summary>
        /// Send the initilization commands to the ELM327
        /// </summary>
        private async Task SendInitializationCommands()
        {
            await SendCommand("ATZ\r");

            await SendCommand("ATSP6\r");

            await SendCommand("ATH0\r");

            await SendCommand("ATCAF1\r");
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
                var xx = e;
            }

            return result;
        }

        public async Task<double> GetSpeed()
        {
            var result = await new Speed(_reader, _writer).GetValue();

            return result;
        }

        public async Task<double> GetRpm()
        {
            var result = await new RPM(_reader, _writer).GetValue();

            return result;
        }

        public async Task<double> GetEngineTemperature()
        {
            var result = await new EngineTemperature(_reader, _writer).GetValue();

            return result;
        }

        public async Task<double> GetFuelPressure()
        {
            var result = await new FuelPressure(_reader, _writer).GetValue();

            return result;
        }

        public async Task<double> GetThrottlePosition()
        {
            var result = await new ThrottlePosition(_reader, _writer).GetValue();

            return result;
        }

        public async Task<double> GetIntakeAirTemperature()
        {
            var result = await new IntakeAirTemperature(_reader, _writer).GetValue();

            return result;
        }

        public async Task<double> GetFuelTankLevelInput()
        {
            var result = await new FuelTankLevelInput(_reader, _writer).GetValue();

            return result;
        }

        public async Task<AllData> GetAllData()
        {
            var result = new AllData();
            //await Task.Delay(50);
            result.Speed = await new Speed(_reader, _writer).GetValue();
            //await Task.Delay(50);
            result.RPM = await new RPM(_reader, _writer).GetValue();
            //await Task.Delay(50);
            result.FuelTankLevelInput = await new FuelTankLevelInput(_reader, _writer).GetValue();
            //await Task.Delay(50);
            result.IntakeAirTemperature = await new IntakeAirTemperature(_reader, _writer).GetValue();
            //await Task.Delay(50);
            result.EngineTemperature = await new EngineTemperature(_reader, _writer).GetValue();
            //await Task.Delay(50);

            return result;
        }

        public async Task Close()
        {
            if (_streamSocket != null)
            {
                await _streamSocket.CancelIOAsync();
                _streamSocket.Dispose();
                _streamSocket = null;
            }
            _deviceService.Dispose();
            _deviceService = null;
        }
    }
}
