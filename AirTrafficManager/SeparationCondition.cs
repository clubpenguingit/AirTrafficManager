using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirTrafficManager
{
    class SeparationCondition : ISeparationCondition
    {
        public event EventHandler<SepCondEventArgs> WarningEvent;

        protected AirTrafficManagementSystem _atms;
        public SeparationCondition(AirTrafficManagementSystem atms)
        {
            this._atms = atms;
            this._atms.DataReady += DataReceived;
        }


        // NOT DONE YET *********************
        // **********************************
        // **********************************
        // **********************************
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
                        CheckForCondition(t, track);
                    }
                }
                
            }
            
        }

        public void CheckForCondition(Track t1, Track t2)
        {

            if ((t1.Altitude - t2.Altitude) <= 300 &&
                (DistanceCalculator(t1.XCoordinate, t2.XCoordinate, t1.YCoordinate, t2.YCoordinate) <= 5000))
            {
                var args = new SepCondEventArgs();
                args.TimeOfOccurrence = DateTime.Now;
                args.Track1 = t1;
                args.Track2 = t2;

                WarningEvent(this, args);
            }
        }

        private double DistanceCalculator(int XT1, int XT2, int YT1, int YT2)
        {
            return Math.Sqrt((Math.Pow((XT2 - XT1), 2)) + 
                             (Math.Pow((YT2 - YT1), 2)));
        }
    }
}
