using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirTrafficManager;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace ATMSUnitTests
{
    [TestFixture]
    public class RendererUnitTests
    {
        #region Setup
        // Attributes to be used in tests
        private Renderer uut;
        private IWriter mockWriter;
        private IAirTrafficManagementSystem stubAtms;
        private ISeparationCondition stubSepCond;
        private List<Track> trackInput;
        private DateTime date;

        [SetUp]
        public void Setup()
        {
            date = DateTime.Now;

            mockWriter = Substitute.For<IWriter>();
            stubAtms = Substitute.For<IAirTrafficManagementSystem>();
            stubSepCond = Substitute.For<ISeparationCondition>();

            uut = new Renderer(mockWriter, stubSepCond, stubAtms);

            trackInput = new List<Track>();
            trackInput.Add(new Track("123456", 1000, 2000,
                4000, date, 5000, 300));
            
        }

        #endregion


        #region RenderAirCraftsTests


        [Test]
        public void RenderAirCrafts_GetsOneTrack_RendersCorrectly()
        {
            //Act
            uut.RenderAirCrafts(trackInput);
            //Assert
            mockWriter.Received(1).Write($"Tag: {trackInput[0].Tag}\n" +
                                         $"Coordinates: ({trackInput[0].XCoordinate} , {trackInput[0].YCoordinate}) meters\n" +
                                         $"Altitude: {trackInput[0].Altitude} meters\n" +
                                         $"Velocity: {trackInput[0].Velocity} m/s\n" +
                                         $"Compass course: {trackInput[0].CompassCourse}\n");
        }

        [Test]
        public void RenderAirCrafts_GetTwoTracks_RendersBothCorrectly()
        {
            // Arrange - Add extra track to list. 
            trackInput.Add(new Track("098765", 46, 100, 1000, DateTime.MaxValue, 1000, 359));

            // Act - Render both tracks
            uut.RenderAirCrafts(trackInput);

            // Assert - Received 2 different calls to write to screen
            mockWriter.Received(2).Write(Arg.Any<string>());
        }

        [Test]
        public void RenderAirCraft_CallWithNoClear_DoesNotClear()
        {
            bool clear = false;
            uut.RenderAirCrafts(trackInput, clear);

            mockWriter.Received(0).ClearConsole();
        }

        [Test]
        public void RenderAirCraft_CallWithClear_DoesClear()
        {
            bool clear = true;
            uut.RenderAirCrafts(trackInput, clear);

            mockWriter.Received(1).ClearConsole();
        }

        [Test]
        public void RenderAirCraft_CallWithoutSecondParam_DoesClearDefault()
        {
            uut.RenderAirCrafts(trackInput);
            mockWriter.Received(1).ClearConsole();
        }
        #endregion


        #region RenderConditionTests

        [Test]  // Three is outputting all the text needed for rendering 
                // 2 for rendering aircrafts, 1 for error msg. 
        public void RenderCondition_RenderTwoTracks_ReceiveThreeCalls()
        {
            // Arrange - Add new track to list (contains total of 2 then)
            trackInput.Add(new Track("098765", 46, 100, 1000, DateTime.MaxValue, 1000, 359));

            uut.RenderCondition(trackInput[0], trackInput[1], DateTime.Now);
            mockWriter.Received(3).Write(Arg.Any<string>());
        }

        [Test]                 //Three is outputting all the text correctly
        public void RenderCondition_RenderTwoTracks_ReceiveNoClear()
        {
            // Arrange - Add new track to list (contains total of 2 then)
            trackInput.Add(new Track("098765", 46, 100, 1000, DateTime.MaxValue, 1000, 359));

            uut.RenderCondition(trackInput[0], trackInput[1] , DateTime.Now);
            mockWriter.Received(0).ClearConsole();

        }


        #endregion

        #region ATMSEventTests

        [Test]
        public void OnDataReadyInATMS_EventInvokedInStub_EventIsReceivedByRendererAndWrittenByWriter()
        {
            var args = new ATMSEventArgs();
            args.Tracks = trackInput;
            stubAtms.DataReady += Raise.EventWith(this, args);

            mockWriter.Received(1).Write($"Tag: {trackInput[0].Tag}\n" +
                                         $"Coordinates: ({trackInput[0].XCoordinate} , {trackInput[0].YCoordinate}) meters\n" +
                                         $"Altitude: {trackInput[0].Altitude} meters\n" +
                                         $"Velocity: {trackInput[0].Velocity} m/s\n" +
                                         $"Compass course: {trackInput[0].CompassCourse}\n");
            mockWriter.Received(1).ClearConsole();
        }

        [Test]
        public void OnSepCondition_OneConditionReceived_OutputsOneCondition()
        {
            // Arrange
            // Create everything required for RendEventArgs
            var args = new RendEventArgs();
            var listOfEvents = new List<SepCondEventArgs>();
            var condEventArgs = new SepCondEventArgs();

            condEventArgs.TimeOfOccurrence = DateTime.Now;
            condEventArgs.Track1 = new Track("123456", 10, 100, 1000, DateTime.Now, 1000, 300);
            condEventArgs.Track2 = new Track("098765", 20, 30, 500, DateTime.Now, 4000, 200);
            listOfEvents.Add(condEventArgs);
    
            args.TimeOfEvent = condEventArgs.TimeOfOccurrence;
            args.ListOfCurrentConditions = listOfEvents;

            // Act - Raise the event
            stubSepCond.RendererWarning += Raise.EventWith(this, args);
            
            // Assert - mockWriter has received calls from uut
            mockWriter.Received(3).Write(Arg.Any<string>());
        }

        [Test]
        public void OnSepCondition_TwoConditionsReceived_OutputsTwoConditions()
        {
            // Arrange
            // Create everything required for RendEventArgs
            var args = new RendEventArgs();
            var listOfEvents = new List<SepCondEventArgs>();
            var condEventArgs = new SepCondEventArgs();

            condEventArgs.TimeOfOccurrence = DateTime.Now;
            condEventArgs.Track1 = new Track("123456", 10, 100, 1000, DateTime.Now, 1000, 300);
            condEventArgs.Track2 = new Track("098765", 20, 30, 500, DateTime.Now, 4000, 200);
            listOfEvents.Add(condEventArgs);

            var condEventArgs2 = new SepCondEventArgs();
            condEventArgs2.TimeOfOccurrence = DateTime.MaxValue;
            condEventArgs2.Track1 = new Track("586038", 2364, 23456, 94865, DateTime.MaxValue, 1094, 224);
            condEventArgs2.Track2 = new Track("985736", 9586, 4956, 2345, DateTime.MinValue, 0, 0);
            listOfEvents.Add(condEventArgs2);

            args.TimeOfEvent = DateTime.Now;
            args.ListOfCurrentConditions = listOfEvents;

            // Act - Raise the event
            stubSepCond.RendererWarning += Raise.EventWith(this, args);

            // Assert - mockWriter has received calls from uut
            mockWriter.Received(6).Write(Arg.Any<string>());
        }

        #endregion

    }



}
