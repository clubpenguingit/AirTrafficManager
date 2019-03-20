using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Remoting.Services;

namespace AirTrafficManager
{
    public interface ITransponderReceiverClient
    {
        //Event AirTrafficManagementSystem will subscribe to
        event EventHandler<DataEventArgs> DataReceivedEvent;
    }
}