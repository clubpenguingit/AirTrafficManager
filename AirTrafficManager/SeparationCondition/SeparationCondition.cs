using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace AirTrafficManager
{
    public class SeparationCondition : ISeparationCondition
    {
        public event EventHandler<SepCondEventArgs> WarningEvent;
        private List<List<Track>> _listOfSepcondTracks;

        protected IAirTrafficManagementSystem _atms;
        public SeparationCondition(IAirTrafficManagementSystem atms)
        {
            this._atms = atms;
            this._atms.DataReady += DataReceived;
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
                        if (CheckIfInList(t, track) != false)
                        {
                            CheckForCondition(t, track);
                        }
                        else if (CheckIfInList(t,track) == true)
                        {
                            RemoveIfNoLongerCondition(t, track);
                        }
                    }
                }
            }
        }

        private void RemoveIfNoLongerCondition(Track track1, Track track2)
        {
            if ((track1.Altitude - track2.Altitude) <= 300 &&
                (DistanceCalculator(track1, track2) <= 5000))
            {
                return;
            }
            else
            {
                var templist = new List<Track>();
                templist.Add(track1);
                templist.Add(track2);
                _listOfSepcondTracks.Remove(templist);
                templist.Reverse();
                _listOfSepcondTracks.Remove(templist);
            }


        }

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
               
                // Raise event for warnings
                WarningEvent(this, args);
            }
        }

        private double DistanceCalculator(Track track1, Track track2)
        {
            var XT2 = track2.XCoordinate;
            var XT1 = track1.XCoordinate;
            var YT2 = track2.YCoordinate;
            var YT1 = track1.YCoordinate;
            return Math.Sqrt((Math.Pow((XT2 - XT1), 2)) + 
                             (Math.Pow((YT2 - YT1), 2)));
        }

        private void AddToTrackList(Track t1, Track t2)
        {
            var templist = new List<Track>();
            templist.Add(t1);
            templist.Add(t2);

            _listOfSepcondTracks.Add(templist);

            templist.Reverse();
            _listOfSepcondTracks.Add(templist);
            
        }

        private bool CheckIfInList(Track a, Track b)
        {
            var tempList = new List<Track>();
            tempList.Add(a);
            tempList.Add(b);
           
            if (_listOfSepcondTracks.Contains(tempList))
            {
                return true;
            }

            tempList.Reverse();
            if (_listOfSepcondTracks.Contains(tempList))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
