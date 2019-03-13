using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirTrafficManager
{
    public class TrackCalculator
    {
        public virtual double CalculateVelocity(int x1, int x2, int y1, int y2, int z1, int z2, int time1, int time2)
        {
            var value = (((x2 - x1) ^ 2) + ((y2 - y1) ^ 2) + ((z2 - z1) ^ 2));
            var deltaDistance = Math.Sqrt(value);

            var deltaTime = time2 - time1;

            return (deltaDistance / deltaTime);
        }

        public virtual double CalculateCourse(int x1, int x2, int y1, int y2)
        {
            //https://www.igismap.com/formula-to-find-bearing-or-heading-angle-between-two-points-latitude-longitude/
            var x = Math.Cos(x2) * Math.Sin(y2 - y1);

            var y = (Math.Cos(x1) * Math.Sin(x2)) - (Math.Sin(x1) * Math.Cos(x2) * Math.Cos(y2 - y1));

            var course = Math.Atan2(x, y);

            return course;
        }
    }
}
