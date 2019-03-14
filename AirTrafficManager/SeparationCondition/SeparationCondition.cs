using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace AirTrafficManager
{
    public class Occurence
    {
        public string Tag1 { get; set; }
        public string Tag2 { get; set; }

        public override bool Equals(object obj)
        {
            var o = obj as Occurence;
            return ((this.Tag1 == o.Tag1) && (this.Tag2 == o.Tag2))
                   || //OR
                   ((this.Tag2 == o.Tag1) && (this.Tag1 == o.Tag2));
        }
    }

    public class SeparationCondition : ISeparationCondition
    {
        public event EventHandler<SepCondEventArgs> WarningEvent;
        public event EventHandler<RendEventArgs> RendererWarning;

        private List<Occurence> _listOfConditionTracks;
        private List<SepCondEventArgs> _SepCondEventArgsList;

        protected IAirTrafficManagementSystem _atms;

        public SeparationCondition(IAirTrafficManagementSystem atms)
        {
            this._atms = atms;
            this._atms.DataReady += DataReceived;
            _listOfConditionTracks = new List<Occurence>();
            _SepCondEventArgsList = new List<SepCondEventArgs>();
        }


        private void DataReceived(object sender, ATMSEventArgs e)
        {
            //For each track 
            foreach (var track in e.Tracks)
            {
                // Compare with all other tracks
                foreach (var t in e.Tracks)
                {
                    if (t != track)
                    {
                        if (CheckIfInList(t, track) == false)
                        {
                            CheckForCondition(t, track);
                        }
                        else if (CheckIfInList(t,track))
                        {
                            RemoveIfNoLongerCondition(t, track);
                        }
                    }
                }
            }

            // Creation of args for renderer event
            var args = new RendEventArgs();
            args.listOfCurrentConditions = _SepCondEventArgsList;
            args.TimeOfEvent = DateTime.Now;
            
            RendererWarning(this, args);
        }


        // Removes two tracks from the list of conditions, if they're no longer in danger
        private void RemoveIfNoLongerCondition(Track track1, Track track2)
        {
            if ((track1.Altitude - track2.Altitude) <= 300 &&
                (DistanceCalculator(track1, track2) <= 5000))
            {
                return;
            }
            else
            {
                var removeHelper = new Occurence();
                removeHelper.Tag1 = track1.Tag;
                removeHelper.Tag2 = track2.Tag;
                _listOfConditionTracks.Remove(removeHelper);
            }
        }


        // Checks if there's a separation condition between two tracks
        public void CheckForCondition(Track t1, Track t2)
        {

            if ((t1.Altitude - t2.Altitude) <= 300 &&
                (DistanceCalculator(t1, t2) <= 5000))
            {

                // Prepare arguments
                var args = new SepCondEventArgs();
                args.TimeOfOccurrence = DateTime.Now;
                args.Track1 = t1;
                args.Track2 = t2;

                AddToTrackList(t1, t2);
                _SepCondEventArgsList.Add(args);
               
                // Raise event for warnings
                WarningEvent(this, args);
            }
        }


        // Calculates the distance between two tracks
        private double DistanceCalculator(Track track1, Track track2)
        {
            var XT2 = track2.XCoordinate;
            var XT1 = track1.XCoordinate;
            var YT2 = track2.YCoordinate;
            var YT1 = track1.YCoordinate;
            return Math.Sqrt((Math.Pow((XT2 - XT1), 2)) + 
                             (Math.Pow((YT2 - YT1), 2)));
        }


        // Adds to the list of tracks that has separation conditions
        private void AddToTrackList(Track t1, Track t2)
        {
            var tempocc = new Occurence();
            tempocc.Tag1 = t1.Tag;
            tempocc.Tag2 = t2.Tag;

            _listOfConditionTracks.Add(tempocc);

        }


        // Checks if two tracks are in the list of separation conditions in both ways
        private bool CheckIfInList(Track a, Track b)
        {
            var tempocc = new Occurence();
            tempocc.Tag1 = a.Tag;
            tempocc.Tag2 = b.Tag;
            foreach (var occ in _listOfConditionTracks)
            {
                if (occ.Equals(tempocc))
                {
                    return true;
                }
            }
            
            return false;
            
        }
    }
}
