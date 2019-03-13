using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransponderReceiver;

namespace AirTrafficManager
{
    public class ATMSEventArgs : EventArgs
    {
        public List<Track> Tracks { get; set; }
    }

    public class AirTrafficManagementSystem
    {

        private MonitoredAirspace _monitoredAirspace;
        private IRenderer _renderer;
        private ITransponderReceiver _transponderReceiver;

        public event EventHandler<ATMSEventArgs> DataReady;

        public AirTrafficManagementSystem(MonitoredAirspace monair, IRenderer rend, ITransponderReceiver transclient)
        {
            this._monitoredAirspace = monair;
            this._renderer = rend;
            this._transponderReceiver = transclient;
        }


    }
}
