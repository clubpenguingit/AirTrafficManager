using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using AirTrafficManager;
using AirTrafficManager.Log;
using NSubstitute;
using TransponderReceiver;

namespace ATMSUnitTests
{
    class SeperationConditionTests
    {
        private AirTrafficManagementSystem _atms; //
        private MonitoredAirspace _monair; //
        private Renderer _irend; //
        private TransponderReceiverClient _itrc;//
        private IWriter _writer; //
        private ITransponderReceiver _itreceiver; //
        private TrackCalculator _calc;//

        private ATMSEventArgs _argsReceived;
        private ATMSEventArgs _argsToSend;

        private List<Track> _tracklist;

        [SetUp]
        public void Setup()
        {
            _argsReceived = null;

            // Creation of all the required classes
            _monair = new MonitoredAirspace(
                95000, 5000, 
                95000, 5000, 
                20000, 500);

            _writer = Substitute.For<IWriter>();
            _itreceiver = Substitute.For<ITransponderReceiver>();
            _calc = Substitute.For<TrackCalculator>();
            _itrc = new TransponderReceiverClient(_itreceiver, _calc);
            _irend = new Renderer(_writer);

            _atms = new AirTrafficManagementSystem(_monair,_irend, _itreceiver);

            var sepcond = new SeparationCondition(_atms);


            // Event creation
            _tracklist = new List<Track>();
            _tracklist.Add(new Track("123456", 50000, 50000, 10000, DateTime.MaxValue, 150, 90));
            _tracklist.Add(new Track("654321", 49000, 50000, 10000, DateTime.MaxValue, 150, 90));

            _argsToSend = new ATMSEventArgs { Tracks = _tracklist };

            _atms.DataReady += (o, args) =>
            {
                _argsReceived = args;
            };

        }

        [Test]
        public void ReceivedEvent()
        {
            _atms.DataReady += Raise.EventWith(_argsToSend);


        }
    }
}
