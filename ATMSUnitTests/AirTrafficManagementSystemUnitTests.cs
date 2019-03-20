using System;
using System.Collections.Generic;
using System.Linq;
using AirTrafficManager;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace ATMSUnitTests
{
    [TestFixture]
    public class AirTrafficManagementSystemUnitTests
    {
        private AirTrafficManagementSystem uut;
        private ITransponderReceiverClient stubTransRecClient;
        private ITrackCalculator stubTrackCalculator;
        private MonitoredAirspace airspace;
        private DataEventArgs ArgsToSend;
        private Track trackToSend;

        [SetUp]
        public void Setup()
        {
            // Mocks, stubs and UUT
            stubTransRecClient = Substitute.For<ITransponderReceiverClient>();
            stubTrackCalculator = Substitute.For<ITrackCalculator>();
            airspace = new MonitoredAirspace(90000,80000,90000,80000,20000,10000); 
            uut = new AirTrafficManagementSystem(airspace, stubTransRecClient, stubTrackCalculator);

            // Data sent from stubTransRecClient
            trackToSend = new Track("123456", 90000, 80000, 15000, DateTime.Now, 0, 0);
            List<Track> listToSend = new List<Track>();
            listToSend.Add(trackToSend);
            ATMSEventArgs CorrectArgs = new ATMSEventArgs();
            CorrectArgs.Tracks = listToSend;

        }

        [Test]
        public void OnReceiverClientEvent_ReceiveEventWith1TrackNotInList_PassItOnCorrectly()
        {
            // Arrange - watch what uut passes on
            List<Track> correctList = new List<Track>();
            correctList.Add(new Track(trackToSend.Tag, trackToSend.XCoordinate, trackToSend.YCoordinate,
                trackToSend.Altitude,trackToSend.Time,trackToSend.Velocity, trackToSend.CompassCourse));

            // Arguments sent
            ArgsToSend = new DataEventArgs(correctList);
            
            // Arguments that should be received
            ATMSEventArgs argsPassedOn = new ATMSEventArgs();

            // Catch the event passed on
            uut.DataReady += (sender, args) =>
            {
                argsPassedOn.Tracks = args.Tracks;
            };

            // Act - Raise the event
            stubTransRecClient.DataReceivedEvent += Raise.EventWith(ArgsToSend);

            // Assert - Check if the argspassed on matches the one that should be sent. 
            Track passedOnTrack = argsPassedOn.Tracks[0];
            Assert.That(passedOnTrack.Tag, Is.EqualTo(trackToSend.Tag));
        }

        [Test]
        public void OnReceiverClientEvent_ReceiveEventWith1TrackNotInListAndOutsideAirspace_ShouldNotBePassedOn()
        {
            // Arrange - track that is outside airspace
            var trackOutsideAirspace = new Track("098765", 0, 10, 123, DateTime.Now, 0, 0);
            var listToSend = new List<Track>();
            listToSend.Add(trackOutsideAirspace);
            DataEventArgs argsToSend = new DataEventArgs(listToSend);

            // Catch arguments passed on
            ATMSEventArgs argsPassedOn = new ATMSEventArgs();
            uut.DataReady += (sender, args) =>
            {
                argsPassedOn = args;
            };
            
            // Act
            stubTransRecClient.DataReceivedEvent += Raise.EventWith(argsToSend);

            // Assert
            Assert.That(argsPassedOn.Tracks.Count, Is.EqualTo(0));
        }

        [Test] // Add velocity e.g.
        public void OnReceiverClientEvent_ReceiveEventWithSameTrackTwice_TrackCorrectly()
        {
            DateTime nowTime = DateTime.Now;
            var firstTrack = new Track("123456", 80000, 82000, 15000, nowTime, 0,0);
            DateTime futureTime = DateTime.Now;
            var secondTrack     = new Track("123456", 81000, 83000, 16000, futureTime, 0,0);
            var correctTrack    = new Track("123456", 81000, 83000, 16000, futureTime, 100, 220);

            List<Track> list = new List<Track>();
            list.Add(firstTrack);
            DataEventArgs argsToSend = new DataEventArgs(list);

            ATMSEventArgs receivedArgs = new ATMSEventArgs();

            // Will save the latest received, should save list with only correctTrack.
            uut.DataReady += (sender, args) =>
            {
                receivedArgs = args;
            };

            // Return correct velocity and course
            stubTrackCalculator
                .CalculateCourse(80000, 81000, 82000, 83000)
                .Returns(220);
            stubTrackCalculator
                .CalculateVelocity(80000, 81000, 82000, 83000, 15000, 16000, Arg.Any<double>(), Arg.Any<double>())
                .Returns(100);
            // Act
            // Run firstTrack through ATMS
            stubTransRecClient.DataReceivedEvent += Raise.EventWith(argsToSend);

            // Send secondTrack through ATMS 
            list.RemoveRange(0, 1);
            list.Add(secondTrack);
            argsToSend.Tracks = list;
            stubTransRecClient.DataReceivedEvent += Raise.EventWith(argsToSend);

            Track resultTrack = receivedArgs.Tracks[0];

            // Assert
            Assert.That(resultTrack.Tag,            Is.EqualTo(correctTrack.Tag));
            Assert.That(resultTrack.Altitude,       Is.EqualTo(correctTrack.Altitude));
            Assert.That(resultTrack.CompassCourse,  Is.EqualTo(correctTrack.CompassCourse));
            Assert.That(resultTrack.Time,           Is.EqualTo(correctTrack.Time));
            Assert.That(resultTrack.Velocity,       Is.EqualTo(correctTrack.Velocity));
            Assert.That(resultTrack.XCoordinate,    Is.EqualTo(correctTrack.XCoordinate));
            Assert.That(resultTrack.YCoordinate,    Is.EqualTo(correctTrack.YCoordinate));
        }

        [Test]
        public void REEEEEEE()
        {
            Track track = new Track("123456", 80000, 80000, 15000, DateTime.MinValue, 0, 0);
            List<Track> list = new List<Track>();
            list.Add(track);
            DataEventArgs toReceivEventArgs = new DataEventArgs(list);

            ATMSEventArgs argsReceived;
            uut.DataReady += (sender, args) =>
            {
                argsReceived = args;

            };

            //Act
            stubTransRecClient.DataReceivedEvent += Raise.EventWith(toReceivEventArgs);
            list.RemoveAt(0);

            list.Add(new Track("123456", 80200, 80200, 15001, DateTime.Now, 0, 0));
            toReceivEventArgs.Tracks = list;
            stubTransRecClient.DataReceivedEvent += Raise.EventWith(toReceivEventArgs);
            list.RemoveAt(0);
            list.Add(new Track("123456", 80900, 80900, 15002, DateTime.MaxValue, 0, 0));
            toReceivEventArgs.Tracks = list;
            stubTransRecClient.DataReceivedEvent += Raise.EventWith(toReceivEventArgs);

           
        }

    }
}