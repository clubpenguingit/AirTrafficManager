using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirTrafficManager;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace ATMSUnitTests
{
    [TestFixture]
    public class MonitoredAirspaceUnitTests
    {
        private MonitoredAirspace _uutAirspace;
        private Track _testTrackTrue;
        private Track _testTrackFalse;


        [SetUp]
        public void Setup()
        {
            _uutAirspace = new MonitoredAirspace(95000,5000,95000,5000,20000,500);
            _testTrackTrue = new Track("True",80000,80000,10000,300,0);
            
        }

        [TestCase(95000, 5000, 95000, 5000)]
        [TestCase(85000, 6000, 85000, 6000)]
        [TestCase(75000, 7000, 75000, 7000)]
        public void SetMethodXY_InRange(int upper, int lower, int upperResult, int lowerResult)
        {
            _uutAirspace.UpperBoundX = upper;
            _uutAirspace.LowerBoundX = lower;
            _uutAirspace.UpperBoundY = upper;
            _uutAirspace.LowerBoundY = lower;
            Assert.That(_uutAirspace.UpperBoundX, Is.EqualTo(upperResult));
            Assert.That(_uutAirspace.LowerBoundX, Is.EqualTo(lowerResult));
            Assert.That(_uutAirspace.UpperBoundY, Is.EqualTo(upperResult));
            Assert.That(_uutAirspace.LowerBoundY, Is.EqualTo(lowerResult));
        }

        [TestCase(20000,500,20000,500)]
        [TestCase(15000,1000,15000,1000)]
        [TestCase(10000,1500,10000,1500)]
        public void SetMethodAltitude_InRange(int upper, int lower, int upperResult, int lowerResult)
        {
            _uutAirspace.UpperAltitudeBound = upper;
            _uutAirspace.LowerAltitudeBound = lower;
            Assert.That(_uutAirspace.UpperAltitudeBound, Is.EqualTo(upperResult));
            Assert.That(_uutAirspace.LowerAltitudeBound, Is.EqualTo(lowerResult));

        }

        [TestCase(95500,4500)]
        [TestCase(100000,4000)]
        [TestCase(95000,0)]
        [TestCase(0,5000)]
        [TestCase(-1,-10)]
        [TestCase(100000,-1)]
        public void SetMethodXY_OutOfRange(int upper, int lower)
        {
          
            Assert.That(() =>
            {
                _uutAirspace.UpperBoundX = upper;
                _uutAirspace.LowerBoundX = lower;
            }, Throws.TypeOf<ArgumentOutOfRangeException>());

            Assert.That(() =>
            {
                _uutAirspace.UpperBoundY = upper;
                _uutAirspace.LowerBoundY = lower;
            }, Throws.TypeOf<ArgumentOutOfRangeException>());
        }


        [TestCase(20000, 0)]
        [TestCase(0, 500)]
        [TestCase(-1, -10)]
        public void SetMethodAltitude_OutRange(int upper, int lower)
        {
            Assert.That(() =>
            {
                _uutAirspace.UpperAltitudeBound = upper;
                _uutAirspace.LowerAltitudeBound = lower;
            }, Throws.TypeOf<ArgumentOutOfRangeException>());

        }


        [Test]
        public void ValidateAirspace_True()
        {
            Assert.That(_uutAirspace.ValidateAirspace(_testTrackTrue), Is.True);
        }

        [TestCase(100000,-1,10000)]
        [TestCase(80000,95500,10000)]
        [TestCase(80000,80000,25000)]
        public void ValidateAirspace_False(int x, int y, int alt)
        {
            _testTrackFalse = new Track("False",x,y,alt,300,0);
            Assert.That(_uutAirspace.ValidateAirspace(_testTrackFalse),Is.False);
        }        
    }
}