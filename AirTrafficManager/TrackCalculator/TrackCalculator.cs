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
            var tempx1 = x1 / 1000;
            var tempx2 = x2 / 1000;
            var tempy1 = y1 / 1000;
            var tempy2 = y2 / 1000;
            var tempz1 = z1 / 1000;
            var tempz2 = z2 / 1000;
            var value = ((Math.Pow((tempx2 - tempx1),2)) + (Math.Pow((tempy2 - tempy1), 2)) + (Math.Pow((tempz2 - tempz1), 2)));
            
            var deltaDistance = Math.Sqrt(value);

            var deltaTime = time2 - time1;

            var velocity = Math.Abs(deltaDistance*1000 / deltaTime);

            return velocity;
        }

        public double CalculateCourse(int x1, int x2, int y1, int y2)
        {
            var bearing = Math.Atan2((y2 - y1), (x2 - x1));

            var bearingDegrees = bearing * 180 / Math.PI;

            if (x2 - x1 < 0 && y2 - y1 >= 0)
            {
                var courseDegrees = 450 - bearingDegrees;
                return courseDegrees;
            }
            else
            {
                var courseDegrees = 90 - bearingDegrees;
                return courseDegrees;
            }
        }
    }
}
