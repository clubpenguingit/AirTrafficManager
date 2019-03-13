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
        private TrackCalculator uut;

        [Test]
        public void CalculateVelocity_OneMeterInOneSecond_Returns_OneMeterPerSecond()
        {
            TrackCalculator tc = new TrackCalculator();
            Assert.That(tc.CalculateVelocity(0, 1, 0, 0, 0, 0, 0, 1), Is.EqualTo(1));
        }

        [TestCase(1, 2, 1, 2, 45)]
        [TestCase(0, 1, 0, 0, 0)]
        [TestCase(-1, 1, -1, 1, 45)]

        public void CalculateCourseTest(int x1, int x2, int y1, int y2, double result)
        {
            TrackCalculator tc = new TrackCalculator();
            Assert.That(tc.CalculateCourse(x1, x2, y1, y2), Is.EqualTo(result).Within(0.1));
        }
    }
}
