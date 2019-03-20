using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using TransponderReceiver;

namespace AirTrafficManager
{
    public class TransponderReceiverClient  : ITransponderReceiverClient
    {
        //Event AirTrafficManagementSystem will subscribe to
        public event EventHandler<DataEventArgs> DataReceivedEvent;
        private ITransponderReceiver receiver;
        private ITrackCalculator calculator;

        // Using constructor injection for dependency/ies
        public TransponderReceiverClient(ITransponderReceiver receiver, ITrackCalculator calculator)
        {
            // This will store the real or the fake transponder data receiver
            this.receiver = receiver;

            this.calculator = calculator;

            // Attach to the event of the real or the fake TDR
            this.receiver.TransponderDataReady += ReceiverOnTransponderDataReady;
        }

        // Event: Receiving Transponder data. Creates a list of tracks from it
        private void ReceiverOnTransponderDataReady(object sender, RawTransponderDataEventArgs e)
        {
            var tracks = new List<Track>();
            // Add all tracks to list
            foreach (var data in e.TransponderData)
            {
                // Split data into separate strings
                string[] trackInfo = data.Split(';');

                // Create DateTime object from trackInfo. 
                int timeIndex = 4;
                string timeString = trackInfo[timeIndex].Substring(0, 4) + "-" + // 'yyyy' 
                                    trackInfo[timeIndex].Substring(4, 2) + "-" + // 'mm'
                                    trackInfo[timeIndex].Substring(6, 2) + " " + // 'dd
                                    trackInfo[timeIndex].Substring(8, 2) + ":" + // 'hh'
                                    trackInfo[timeIndex].Substring(10, 2) + ":" + // 'mm'
                                    trackInfo[timeIndex].Substring(12, 2) + "," + // 'ss'
                                    trackInfo[timeIndex].Substring(14, 3);            // 'fff'
                DateTime time = DateTime.ParseExact(timeString, "yyyy-MM-dd HH:mm:ss,fff",
                                                        System.Globalization.CultureInfo.InvariantCulture);

                // Create track from split data
                var t = new Track(trackInfo[0], int.Parse(trackInfo[1]),
                                int.Parse(trackInfo[2]),int.Parse(trackInfo[3]),
                                    time, 0
                                , 0);
                //Velocity and compassCourse will be calculated in  ATM
                // Add track to list
                tracks.Add(t); 
            }
            
            DataReceivedEvent?.Invoke(this, new DataEventArgs(tracks));
        }
    }
}