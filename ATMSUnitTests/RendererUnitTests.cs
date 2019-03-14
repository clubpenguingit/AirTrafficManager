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
        private List<Track> trackInput;
        private DateTime date;

        [SetUp]
        public void Setup()
        {
            date = DateTime.Now;
            mockWriter = Substitute.For<IWriter>();
            uut = new Renderer(mockWriter);
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
                                         $"Velocity: {trackInput[0].Velocity} meters\n" +
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

        //[Test]
        //public void OnDataReadyInATMS_EventInvokedInStub_EventIsReceivedByRenderer()
        //{

        //}

        #endregion

    }



}
