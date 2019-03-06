using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirTrafficManager
{
    public class Track
    {
        public string Tag { get; private set; }
        public int XCoordinate { get; private set; }
        public int YCoordinate { get; private set; }
        public int Altitude { get; private set; }
        public int Velocity { get; private set; }
        public int CompassCourse { get; private set; }

    }
}
