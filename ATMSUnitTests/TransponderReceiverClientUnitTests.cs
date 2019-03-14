using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using NUnit.Framework;
using NUnit.Framework.Internal;
using NSubstitute;
using AirTrafficManager;
using TransponderReceiver;

namespace ATMSUnitTests
{
    [TestFixture]
    public class TransponderReceiverClientUnitTests
    {
        private TransponderReceiverClient uut;
        private ITrackCalculator fakeCalculator;
        private ITransponderReceiver fakeReceiver;
        private int numberOfEventsReceived;

        [SetUp]
        public void Setup()
        {
            // Create fakes
            fakeReceiver = Substitute.For<ITransponderReceiver>();
            fakeCalculator = Substitute.For<ITrackCalculator>();

            // Create unit under test
            uut = new TransponderReceiverClient(fakeReceiver, fakeCalculator);
            int numberOfEventsReceived = 0;
        }


        //[Test]
        //public void TransponderReceiverClient_fakeReceiverRaisesEvent_uutReceivesEvent()
        //{
        //    // Create testdata
        //    List<string> testData = new List<string>();
        //    testData.Add("ATR423;39045;12932;14000;20151006213456789");
        //    testData.Add("BCD123;10005;85890;12000;20151006213456789");
        //    testData.Add("XYZ987;25059;75654;4000;20151006213456789");

        //    int numberOfEventsReceived = 0;
        //    uut.DataReceivedEvent += (object s, DataEventArgs e) =>
        //    {
        //        numberOfEventsReceived++;
        //    };
        //    // Act
        //    fakeReceiver.TransponderDataReady += Raise.EventWith(this, new RawTransponderDataEventArgs(testData));
        //    // Assert
        //    Assert.That(numberOfEventsReceived, Is.EqualTo(1));
        //}

        //[Test]
        //public void TransponderReceiverClient_fakeReceiverRaisesSpecificData_uutCreatesTrack()
        //{
        //    // Arrange
        //    List<string> testData = new List<string>();
        //    testData.Add("ATR423;39045;12932;14000;20151006213456789");

        //    Track resultTrack = null;
        //    Track correctTrack = new Track("ATR423", 39045,12932, 14000, 
        //                                    new DateTime(2015,10,06, 21,34,
        //                                        56,789),0, 0 );
        //    uut.DataReceivedEvent += (object s, DataEventArgs e) =>
        //    {
        //        resultTrack = e.Tracks[0];
        //    };

        //    // Act 
        //    fakeReceiver.TransponderDataReady += Raise.EventWith(this, new RawTransponderDataEventArgs(testData));

        //    // Assert that all properties are the same (did not work with simple object == object) 
        //    Assert.That(resultTrack.Tag,            Is.EqualTo(correctTrack.Tag));
        //    Assert.That(resultTrack.XCoordinate,    Is.EqualTo(correctTrack.XCoordinate));
        //    Assert.That(resultTrack.YCoordinate,    Is.EqualTo(correctTrack.YCoordinate));
        //    Assert.That(resultTrack.Altitude,       Is.EqualTo(correctTrack.Altitude));
        //    Assert.That(resultTrack.Time,           Is.EqualTo(correctTrack.Time));
        //    Assert.That(resultTrack.Velocity,       Is.EqualTo(correctTrack.Velocity));
        //    Assert.That(resultTrack.CompassCourse,  Is.EqualTo(correctTrack.CompassCourse));
        //}
    }
}