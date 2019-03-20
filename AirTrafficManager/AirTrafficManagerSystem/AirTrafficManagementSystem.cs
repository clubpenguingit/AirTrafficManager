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

    public class AirTrafficManagementSystem : IAirTrafficManagementSystem
    {

        private MonitoredAirspace _monitoredAirspace;
        private ITransponderReceiverClient _transPondRecClient;
        private List<Track> _airCraftsInAirspaceList;
        private ITrackCalculator _trackCalculator;
        private MonitoredAirspace monitoredAirspace;
        private TransponderReceiverClient client;
        private TrackCalculator trackCalculator;

        public event EventHandler<ATMSEventArgs> DataReady;

        public AirTrafficManagementSystem(MonitoredAirspace monair,  ITransponderReceiverClient transclient,
                                            ITrackCalculator calc)
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
                Track trackToValidate = null;
                // If already in list - update speed and coordinates.
                if (foundTrack != null)
                {
                    double trackSeconds = (track.Time.Minute * 60) + track.Time.Second;
                    double foundTrackSeconds = (foundTrack.Time.Minute * 60) + foundTrack.Time.Second;

                    var velocity = _trackCalculator.CalculateVelocity(foundTrack.XCoordinate, track.XCoordinate,
                        foundTrack.YCoordinate, track.YCoordinate,
                        foundTrack.Altitude, track.Altitude,
                        foundTrackSeconds, trackSeconds );

                    var compassCourse = _trackCalculator.CalculateCourse(foundTrack.XCoordinate, track.XCoordinate,
                        foundTrack.YCoordinate, track.YCoordinate);

                    var newTrack = new Track(foundTrack.Tag, track.XCoordinate, track.YCoordinate,
                        track.Altitude, track.Time, velocity, compassCourse);

                    _airCraftsInAirspaceList.Remove(foundTrack);
                    _airCraftsInAirspaceList.Add(newTrack);
                    trackToValidate = newTrack;
                }

                // If not in list - add it    This should be tested - intellisense says expression is always true
                else if (foundTrack == null)
                {
                    _airCraftsInAirspaceList.Add(track);
                    trackToValidate = track;
                }

                // If outside airspace, remove it. 
                bool inAirSpace = _monitoredAirspace.ValidateAirspace(trackToValidate);
                if (!inAirSpace)
                {
                    _airCraftsInAirspaceList.Remove(trackToValidate);
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
