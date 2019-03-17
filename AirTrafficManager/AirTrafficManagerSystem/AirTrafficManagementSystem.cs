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
        private List<Track> _airCraftsInAirspaceList;
        private TrackCalculator _trackCalculator;

        public event EventHandler<ATMSEventArgs> DataReady;

        public AirTrafficManagementSystem(MonitoredAirspace monair,  ITransponderReceiverClient transclient,
                                            TrackCalculator calc)
        {
            this._monitoredAirspace = monair;
            this._transPondRecClient = transclient;
            this._trackCalculator = calc;
            _transPondRecClient.DataReceivedEvent += OnReceiverClientEvent;
            _airCraftsInAirspaceList = new List<Track>();
        }
        

        //When ReceiverClient events
        private void OnReceiverClientEvent(object sender, DataEventArgs e)
        {

            foreach (var track in e.Tracks)
            {
                // Look for track in list of tracks in airspace
                var foundTrack = _airCraftsInAirspaceList.Find(ByTag(track.Tag));

                // If already in list - update speed and coordinates.
                if (foundTrack != null)
                {
                    var velocity = _trackCalculator.CalculateVelocity(track.XCoordinate, foundTrack.XCoordinate,
                        track.YCoordinate, foundTrack.YCoordinate,
                        track.Altitude, foundTrack.Altitude,
                        track.Time.Second, foundTrack.Time.Second); // Second might have to change to ToADate e.g.

                    var compassCourse = _trackCalculator.CalculateCourse(track.XCoordinate, foundTrack.YCoordinate,
                        track.YCoordinate, foundTrack.YCoordinate);

                    var newTrack = new Track(foundTrack.Tag, foundTrack.XCoordinate, foundTrack.YCoordinate,
                        foundTrack.Altitude, track.Time, velocity, compassCourse);
                }
                // If not in list - add it    This should be tested - intellisense says expression is alwais true
                else if (foundTrack == null)
                {
                    _airCraftsInAirspaceList.Add(track);
                }
                // If outside airspace, remove it. 
                bool inAirSpace = _monitoredAirspace.ValidateAirspace(track);
                if (!inAirSpace)
                {
                    _airCraftsInAirspaceList.Remove(track);
                }
            }

            // Pass on Tracks in monitored airspace. 
            var args = new ATMSEventArgs();
            args.Tracks = _airCraftsInAirspaceList;

            // Raise event if somebody has "subscribed" to it
            DataReady?.Invoke(this, args);
        }

        // List.Find helper
        Predicate<Track> ByTag(string tag)
        {
            return delegate(Track track)
            { 
                return track.Tag == tag;
            };
        }
    }
}
