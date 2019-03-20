using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirTrafficManager;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace ATMSUnitTests
{
    [TestFixture]
    public class TrackCalculatorUnitTests
    {
        [TestCase(0, 1, 0, 0, 0, 0, 0, 1, 1)]
        [TestCase(0, 2, 0, 0, 0, 0, 0, 1, 2)]
        [TestCase(0, 2, 0, 0, 0, 0, 0, 2, 1)]
        [TestCase(0, 0, 0, 1, 0, 0, 0, 1, 1)]
        [TestCase(0, 0, 0, 0, 0, 1, 0, 1, 1)]
        [TestCase(0, 0, 0, 1, 0, 0, 1, 2, 1)]
        public void CalculateVelocityTest(int x1, int x2, int y1, int y2, int z1, int z2, double time1, double time2, double result)
        {
            TrackCalculator tc = new TrackCalculator();
            Assert.That(tc.CalculateVelocity(x1, x2, y1, y2, z1, z2, time1, time2), Is.EqualTo(result));
        }

        [TestCase(1, 2, 1, 2, 45)]
        [TestCase(0, 1, 0, 0, 90)]
        [TestCase(-1, 1, -1, 1, 45)]
        [TestCase(0,-1,0,0, 270)]
        [TestCase(0,0,0,-1,180)]
        [TestCase(0,0,0,1,0)]

        public void CalculateCourseTest(int x1, int x2, int y1, int y2, double result)
        {
            TrackCalculator tc = new TrackCalculator();
            Assert.That(tc.CalculateCourse(x1, x2, y1, y2), Is.EqualTo(result).Within(0.1));
        }
    }
}
