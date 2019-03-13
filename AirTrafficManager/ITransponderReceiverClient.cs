using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Remoting.Services;

namespace AirTrafficManager
{
    public class DataEventArgs : EventArgs
    {
        public DataEventArgs(List<Track> tracks)
        {
            Tracks = tracks;
        }
        public List<Track> Tracks { get; set; }
    }

    public interface ITransponderReceiverClient
    {
        //Event AirTrafficManagementSystem will subscribe to
        event EventHandler<DataEventArgs> DataReceivedEvent;
    }
}