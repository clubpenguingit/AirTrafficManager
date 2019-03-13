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
            int alt, DateTime time, double vel, double compC)
        {
            Tag = tag;
            XCoordinate = xcoor;
            YCoordinate = ycoor;
            Altitude = alt;
            Time = time;
            Velocity = vel;
            CompassCourse = compC;
        }

        private string _Tag;
        public string Tag
        {
            get { return _Tag; }
            private set
            {
                if (Tag.Length == 6)
                {
                    _Tag = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Tag was not 6 characters long.");
                }
            }
        }
        public int XCoordinate { get; private set; }
        public int YCoordinate { get; private set; }
        public int Altitude { get; private set; }
        public DateTime Time { get; private set; }
        private double _Velocity;

        public double Velocity
        {
            get { return _Velocity; }
            set
            {
                if (Velocity < 0)
                {
                    throw new ArgumentOutOfRangeException("Velocity cannot be negative.");
                }
                else
                {
                    _Velocity = value;
                }
            }
        }
        private double _CompassCourse;

        public double CompassCourse
        {
            get { return _CompassCourse;}
            set
            {
                if (CompassCourse > 359 || CompassCourse < 0)
                {
                    throw new ArgumentOutOfRangeException("Course was not within range.");
                }
                else
                {
                    _CompassCourse = value;
                }
                //if (CompassCourse > 359)
                //{
                //    _CompassCourse = value % 360;
                //}
                //else if (CompassCourse < 0)
                //{
                //    _CompassCourse = ( 360 - ( value * ( -1 ) ) ) % 360;
                //}
                //else
                //{
                //    _CompassCourse = value;
                //}
            }
        }
    }
}
