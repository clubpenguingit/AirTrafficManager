using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirTrafficManager;
using AirTrafficManager.Log;
using NSubstitute;
using NUnit.Framework;

namespace ATMSUnitTests
{
    public class NormalLoggerUnitTest
    {
        private IInputOutput _fakeInputOutput;
        private ISeparationCondition _fakeSeparationCondition;

        private NormalLogger _uut;
        private SepCondEventArgs _receivedEventArgs;
        private SepCondEventArgs _argsToSend;
        private DateTime date;


        [SetUp]
        public void Setup()
        {
            date = DateTime.Now;
            _receivedEventArgs = null;
            _fakeInputOutput = Substitute.For<IInputOutput>();
            _fakeSeparationCondition = Substitute.For<ISeparationCondition>();


            _argsToSend = new SepCondEventArgs();
            _argsToSend.Track1 = new Track("123456", 1000, 2000, 5000,date, 250, 32);
            _argsToSend.Track2 = new Track("654321", 2000, 1000, 5500,date, 150, 161);
            _argsToSend.TimeOfOccurrence = new DateTime(2019, 3, 25, 23, 59, 00);

            _uut = new NormalLogger("UnitTestFile.txt", _fakeSeparationCondition, _fakeInputOutput);

            _fakeSeparationCondition.WarningEvent += (o, args) =>
            {
                _receivedEventArgs = args;
            };
        }


        [Test]
        public void InputOutput_ReceivesOneWrite_FromArgs()
        {
            _fakeSeparationCondition.WarningEvent += Raise.EventWith(_argsToSend);
            _fakeInputOutput.Received(1).Write(_receivedEventArgs, "UnitTestFile.txt");
        }

    }
}
