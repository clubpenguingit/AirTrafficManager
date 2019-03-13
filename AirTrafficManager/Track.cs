using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirTrafficManager
{
    public class Track
    {
        public Track(string tag, int xcoor, int ycoor,
            int alt, double vel, double compC)
        {
            Tag = tag;
            XCoordinate = xcoor;
            YCoordinate = ycoor;
            Altitude = alt;
            Velocity = vel;
            CompassCourse = compC;
        }

        public string Tag { get; private set; }
        public int XCoordinate { get; private set; }
        public int YCoordinate { get; private set; }
        public int Altitude { get; private set; }
        public double Velocity { get; private set; }
        public double CompassCourse { get; private set; }

    }
}
