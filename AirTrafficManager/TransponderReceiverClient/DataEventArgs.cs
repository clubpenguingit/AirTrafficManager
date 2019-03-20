using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirTrafficManager
{
      public class DataEventArgs : EventArgs
    {
        public List<Track> Tracks { get; set; }

        public DataEventArgs(List<Track> tracks)
        {
            Tracks = tracks;
        }
    }
}
