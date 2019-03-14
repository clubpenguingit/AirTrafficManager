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
        private ITransponderReceiverClient _transPondRecClient;

        public event EventHandler<ATMSEventArgs> DataReady;

        public AirTrafficManagementSystem(MonitoredAirspace monair,  ITransponderReceiverClient transclient)
        {
            this._monitoredAirspace = monair;
            this._transPondRecClient = transclient;
            _transPondRecClient.DataReceivedEvent += OnReceiverClientEvent;
        }
        

        //When ReceiverClient events
        private void OnReceiverClientEvent(object sender, DataEventArgs e)
        {
            // TODO: do
            var airCraftsInAirspaceList = new List<Track>();

            foreach (var track in e.Tracks)
            {
                if (_monitoredAirspace.ValidateAirspace(track))
                {

                }
            }
        }
    }
}
