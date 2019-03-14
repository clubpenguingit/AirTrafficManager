using System;
using AirTrafficManager;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace ATMSUnitTests
{
    [TestFixture]
    public class TrackUnitTests
    {
        private Track _uutTrack;
        private DateTime date;

        [SetUp]
        public void Setup()
        {
            date = DateTime.MaxValue;
            _uutTrack = new Track("Tester", 95000, 50000, 20000, date, 300, 1);
        }


        [TestCase("000000","000000")]
        [TestCase("111111","111111")]
        [TestCase("222222","222222")]
        public void Tag_Accepted(string tag, string expectedTag)
        {
            _uutTrack = new Track(tag,95000,50000,20000,date,300,1);
            Assert.That(_uutTrack.Tag,Is.EqualTo(expectedTag));
        }

        [TestCase("00000")]
        [TestCase("0000000")]
        public void Tag_Rejected(string tag)
        {
            Assert.That(() =>
                    _uutTrack = new Track(tag, 95000, 50000, 20000, date, 300, 1),
                Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void Get_Time()
        {
            Assert.That(_uutTrack.Time,Is.EqualTo(date));
        }

        [TestCase(300,300)]
        [TestCase(1,1)]
        [TestCase(150,150)]
        public void Course_Accepted(double course, double acceptedCourse)
        {
            _uutTrack = new Track("Tester", 95000, 50000, 20000, date, 300, course);
            Assert.That(_uutTrack.CompassCourse,Is.EqualTo(acceptedCourse));
        }

        [TestCase(360)]
        [TestCase(-1)]
        public void Course_Rejected(double course)
        {
            Assert.That(() =>
                    _uutTrack = new Track("Tester", 95000, 50000, 20000, date, 300, course),
                Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void Get_Velocity()
        {
            Assert.That(_uutTrack.Velocity,Is.EqualTo(300));
        }


        [TestCase(300,300)]
        [TestCase(1,1)]
        public void Velocity_Accepted(double velocity, double expectedVelocity)
        {
            _uutTrack = new Track("Tester", 95000, 50000, 20000, date, velocity, 1);
            Assert.That(_uutTrack.Velocity, Is.EqualTo(expectedVelocity));
        }

        [TestCase(-1)]
        [TestCase(-3)]
        public void Velocity_Rejected(double velocity)
        {
            Assert.That(() =>
                    _uutTrack = new Track("Tester", 95000, 50000, 20000, date, velocity, 1),
                Throws.TypeOf<ArgumentOutOfRangeException>());
        }



    }
}