using System;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace TCC.ODBDriver
{
    public sealed class StreamSocketProvider
    {
        public DataReader Reader { get; set; }
        public DataWriter Writer { get; set; }
        public StreamSocket StreamSocket { get; set; }

        private static readonly Lazy<StreamSocketProvider> lazy =
        new Lazy<StreamSocketProvider>(() => new StreamSocketProvider());

        public static StreamSocketProvider Instance { get { return lazy.Value; } }

        private StreamSocketProvider()
        {
        }

    }
}
