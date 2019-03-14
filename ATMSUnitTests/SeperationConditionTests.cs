using System;
using System.Collections.Generic;
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


            // Event creation
            _tracklist = new List<Track>();
            _tracklist.Add(new Track("123456", 50000, 50000, 10000, DateTime.MaxValue, 150, 90));
            _tracklist.Add(new Track("654321", 49000, 50000, 10000, DateTime.MaxValue, 150, 90));

            _argsToSend = new ATMSEventArgs { Tracks = _tracklist };
            _sepCondEventArgs = new SepCondEventArgs();
            _sepCondEventArgs.Track1= new Track("123456", 50000, 50000, 10000, DateTime.MaxValue, 150, 90);
            _sepCondEventArgs.Track2 = new Track("654321", 49000, 50000, 10000, DateTime.MaxValue, 150, 90);
            _sepCondEventArgs.TimeOfOccurrence = DateTime.MaxValue;

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

        [Test]
        public void ReceivedEvent()
        {
            _atms.DataReady += Raise.EventWith(_argsToSend);
            _inputoutput.Received(1).Write(Arg.Any<SepCondEventArgs>(), Arg.Any<string>());




        //}

        /*[Test]
        public void Reeee()
        {

            var log = Substitute.For<ILogger>();
            sepcond.WarningEvent += log.SepConditionOccured;

            _atms.DataReady += Raise.EventWith(_argsToSend);

            log.Received(1).SepConditionOccured(Arg.Any<object>(), Arg.Any<SepCondEventArgs>());
        }*/
    }
}
