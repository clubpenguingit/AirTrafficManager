using System;

namespace AirTrafficManager
{
    public interface IAirTrafficManagementSystem
    {
        event EventHandler<ATMSEventArgs> DataReady;
    }
}