using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirTrafficManager
{
    public class TrackCalculator : ITrackCalculator
    {
        public double CalculateVelocity(int x1, int x2, int y1, int y2, int z1, int z2, double time1, double time2)
        {
            var value = ((Math.Pow((x2 - x1),2)) + (Math.Pow((y2 - y1), 2)) + (Math.Pow((z2 - z1), 2)));
            
            var deltaDistance = Math.Sqrt(value);

            var deltaTime = time2 - time1;

            var velocity = deltaDistance / deltaTime;

            return velocity;
        }

        public double CalculateCourse(int x1, int x2, int y1, int y2)
        {
            var m = (y2 - y1) / (x2 - x1);

            var courseRadians = Math.Atan(m);

            var courseDegrees = courseRadians * (180 / Math.PI);

            return courseDegrees;
        }
    }
}
