using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using AirTrafficManager;
using AirTrafficManager.Log;
using AirTrafficManager.LoggingClasses;
using NSubstitute;
using TransponderReceiver;

namespace ATMSUnitTests
{
    public class SeperationConditionTests
    {
        private IAirTrafficManagementSystem _atms; //
        private IWriter _writer; //
        private NormalLogger _log;
        private IInputOutput _inputoutput;

        private ATMSEventArgs _argsReceived;
        private ATMSEventArgs _argsToSend;
        private SepCondEventArgs _sepCondEventArgs;
        private RendEventArgs _rendEventArgs;
        private SeparationCondition sepcond;
        private RendEventArgs correctRendEventArgs;

        private List<Track> _tracklist;

        [SetUp]
        public void Setup()
        {
            _argsReceived = null;

            // Creation of all the required classes

            _writer = Substitute.For<IWriter>();
            _inputoutput = Substitute.For<IInputOutput>();

            _atms = Substitute.For<IAirTrafficManagementSystem>();

            sepcond = new SeparationCondition(_atms);
            _log = new NormalLogger("testfil", sepcond, _inputoutput);

            // Arguments that sepcond sends to Renderer when it makes an event
            _rendEventArgs = new RendEventArgs();
            var sepcondlist = new List<SepCondEventArgs>();
            sepcondlist.Add(_sepCondEventArgs);
            _rendEventArgs.listOfCurrentConditions = sepcondlist;
            _rendEventArgs.TimeOfEvent = DateTime.Now;
            

            _atms.DataReady += (o, args) =>
            {
                _argsReceived = args;
            };

            sepcond.WarningEvent += (o, args2) =>
            {
                _sepCondEventArgs = args2;
            };

            sepcond.RendererWarning += (o, args3) =>
            {
                _rendEventArgs = args3;
            };
        }
// Tests if a single event is triggered in Sepcond //
        [Test]
        public void SepCondEvent_ReceivesOneEvent_Only1EventSent()
        {
            Track track1 = new Track("123456", 50000, 50000, 10000, DateTime.Now, 150, 90);
            Track track2 = new Track("654321", 49000, 50000, 10000, DateTime.Now, 150, 90);
            _tracklist = new List<Track>();
            _tracklist.Add(track1);
            _tracklist.Add(track2);
            _argsToSend = new ATMSEventArgs { Tracks = _tracklist };

            correctRendEventArgs = new RendEventArgs();
            var sepcondlistforrenderer = new List<SepCondEventArgs>();
            var newsepcond = new SepCondEventArgs();
            newsepcond.Track1 = track1;
            newsepcond.Track2 = track2;
            newsepcond.TimeOfOccurrence = DateTime.Now;
  
            sepcondlistforrenderer.Add(newsepcond);
            correctRendEventArgs.listOfCurrentConditions = sepcondlistforrenderer;
            correctRendEventArgs.TimeOfEvent = DateTime.Now;

            _atms.DataReady += Raise.EventWith(_argsToSend);
            _inputoutput.Received(1).Write(Arg.Any<SepCondEventArgs>(), Arg.Any<string>());
            Assert.That(_rendEventArgs.listOfCurrentConditions[0].Track2, Is.EqualTo(correctRendEventArgs.listOfCurrentConditions[0].Track2));
        }


 // Covers code of the planes being removed from the list in SeperationCondition/////
        [Test]
        public void SepCondEvent_Receives2EventsAfter1stPlanesMovesOutOfSepCond_Only1EventSentFromSetup()
        {
            //First set in setup
            Track track1 = new Track("123456", 50000, 50000, 10000, DateTime.Now, 150, 90);
            Track track2 = new Track("654321", 49000, 50000, 10000, DateTime.Now, 150, 90);
            _tracklist = new List<Track>();
            _tracklist.Add(track1);
            _tracklist.Add(track2);
            _argsToSend = new ATMSEventArgs { Tracks = _tracklist };
            _atms.DataReady += Raise.EventWith(_argsToSend);

            //Second set
            _tracklist = new List<Track>();
            _tracklist.Add(new Track("123456", 50000, 50000, 10000, DateTime.Now, 150, 90));
            _tracklist.Add(new Track("654321", 30000, 30000, 9000, DateTime.Now, 150, 180));

            _argsToSend = new ATMSEventArgs { Tracks = _tracklist };

            _atms.DataReady += Raise.EventWith(_argsToSend);
            _inputoutput.Received(1).Write(Arg.Any<SepCondEventArgs>(), Arg.Any<string>());
        }


// Sends two events because of four conflicting planes
        [Test]
        public void SepCondEvent_Receives2Events_Sends2Events()
        {
            //First set in setup
            Track track1 = new Track("123456", 50000, 50000, 10000, DateTime.Now, 150, 90);
            Track track2 = new Track("654321", 49000, 50000, 10000, DateTime.Now, 150, 90);
            _tracklist = new List<Track>();
            _tracklist.Add(track1);
            _tracklist.Add(track2);
            _argsToSend = new ATMSEventArgs { Tracks = _tracklist };
            _atms.DataReady += Raise.EventWith(_argsToSend);

            //Second set
            _tracklist = new List<Track>();
            _tracklist.Add(new Track("987654", 40000, 30000, 9000, DateTime.Now, 150, 180));
            _tracklist.Add(new Track("456789", 40000, 33000, 9000, DateTime.Now, 150, 180));

            _argsToSend = new ATMSEventArgs { Tracks = _tracklist };

            _atms.DataReady += Raise.EventWith(_argsToSend);
            _inputoutput.Received(2).Write(Arg.Any<SepCondEventArgs>(), Arg.Any<string>());
        }

// Doesn't send event when they're already added to the conflicting list
        [Test]
        public void SepCondEvent_AddSameTracksTwiceWithSepCond_Raise1Event()
        {
            //First set in setup
            Track track1 = new Track("123456", 50000, 50000, 10000, DateTime.Now, 150, 90);
            Track track2 = new Track("654321", 49000, 50000, 10000, DateTime.Now, 150, 90);
            _tracklist = new List<Track>();
            _tracklist.Add(track1);
            _tracklist.Add(track2);
            _argsToSend = new ATMSEventArgs { Tracks = _tracklist };
            _atms.DataReady += Raise.EventWith(_argsToSend);

            //Second set
            _tracklist = new List<Track>();
            _tracklist.Add(new Track("123456", 50000, 50000, 10000, DateTime.Now, 150, 90));
            _tracklist.Add(new Track("654321", 49500, 50000, 10000, DateTime.Now, 150, 180));

            _argsToSend = new ATMSEventArgs { Tracks = _tracklist };

            _atms.DataReady += Raise.EventWith(_argsToSend);
            _inputoutput.Received(1).Write(Arg.Any<SepCondEventArgs>(), Arg.Any<string>());
        }



        /* TEST FOR HØJDE EDGE CASES*/
        [TestCase("123456", "654321", 45000, 45000, 5000, 5300, 150, 150, 1)]
        [TestCase("123456", "654321", 45000, 45000, 5000, 5299, 150, 150, 1)]
        [TestCase("123456", "654321", 45000, 45000, 5000, 5301, 150, 150, 0)]
        public void SepCondEvent_AddTracksAtHeightEdgeCases_SendsEvent(String tag1, string tag2, int xcoord, int ycoord, 
                                                            int alt1, int alt2, int vel, int compc, int resultat)
        {
            Track track1 = new Track(tag1, xcoord, ycoord, alt1, DateTime.Now, vel, compc);
            Track track2 = new Track(tag2, xcoord, ycoord, alt2, DateTime.Now, vel, compc);

            // Add to list and prepare ATMS event
            _tracklist = new List<Track>();
            _tracklist.Add(track1);
            _tracklist.Add(track2);
            _argsToSend = new ATMSEventArgs { Tracks = _tracklist };
            _atms.DataReady += Raise.EventWith(_argsToSend);

            _inputoutput.Received(resultat).Write(Arg.Any<SepCondEventArgs>(), Arg.Any<string>());
        }
    }
}
