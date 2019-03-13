using System.Collections.Generic;
using System.Runtime.InteropServices;
using NUnit.Framework;
using NUnit.Framework.Internal;
using NSubstitute;
using AirTrafficManager;
using TransponderReceiver;

namespace ATMSUnitTests
{
    [TestFixture()]
    public class TransponderReceiverClientUnitTests
    {
        private TransponderReceiverClient uut;
        private ITrackCalculator fakeCalculator;
        private ITransponderReceiver fakeReceiver;
               

        [SetUp]
        public void Setup()
        {
            // Create fakes
            fakeReceiver = Substitute.For<ITransponderReceiver>();
            fakeCalculator = Substitute.For<ITrackCalculator>();

            // Create unit under test
            uut = new TransponderReceiverClient(fakeReceiver, fakeCalculator);

         
        }


        [Test]
        public void TransponderReceiverClient_fakeReceiverRaisesEvent_uutReceivesEvent()
        {
            // Create testdata
            List<string> testData = new List<string>();
            testData.Add("ATR423;39045;12932;14000;20151006213456789");
            testData.Add("BCD123;10005;85890;12000;20151006213456789");
            testData.Add("XYZ987;25059;75654;4000;20151006213456789");

            int numberOfEventsReceived = 0;
            uut.DataReceivedEvent += (object s, DataEventArgs e) =>
            {
                ++numberOfEventsReceived;
            };
            // Act
            fakeReceiver.TransponderDataReady += Raise.EventWith(this, new RawTransponderDataEventArgs(testData));
            // Assert
            Assert.That(numberOfEventsReceived, Is.EqualTo(1));
        }

        //[Test]
        //public void TransponderReceiverClient_fakeReceiverRaisesSpecificData_uutCreatesTrack()
        //{
        //    // Arrange
        //    List<string> testData = new List<string>();
        //    testData.Add("ATR423;39045;12932;14000;20151006213456789");

        //    Track resultTrack; 
        //    Track correctTrack = new Track("ATR423", 39045);
        //    uut.DataReceivedEvent += (object s, DataEventArgs e) =>
        //    {
                
        //    };

        //    // Act 
        //    fakeReceiver.TransponderDataReady += Raise.EventWith(this, new RawTransponderDataEventArgs(testData));
        //}
    }
}