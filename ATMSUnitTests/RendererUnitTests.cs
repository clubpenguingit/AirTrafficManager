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

        private Renderer uut;
        private IWriter mockWriter;
        private Track trackInput;
        private DateTime date;
        [SetUp]
        public void Setup()
        {
            date = DateTime.Now;
            mockWriter = Substitute.For<IWriter>();
            uut = new Renderer(mockWriter);
            trackInput = new Track(
                "123456", 1000, 2000,
                4000,date, 5000, 300);
        }

        #endregion


        #region RenderAirCraftTests


        [Test]
        public void RenderAirCraft_RendersInputCorrectly()
        {
            //Act
            uut.RenderAirCraft(trackInput);
            //Assert
            mockWriter.Received(1).Write($"\nTag: {trackInput.Tag}\n" +
                                         $"Coordinates: ({trackInput.XCoordinate} , {trackInput.YCoordinate}) meters\n" +
                                         $"Altitude: {trackInput.Altitude} meters\n" +
                                         $"Velocity: {trackInput.Velocity} meters\n" +
                                         $"Compass course: {trackInput.CompassCourse}");
        }

        [Test]
        public void RenderAirCraft_CallWithNoClear_DoesNotClear()
        {
            bool clear = false;
            uut.RenderAirCraft(trackInput, clear);

            mockWriter.Received(0).ClearConsole();
        }

        [Test]
        public void RenderAirCraft_CallWithClear_DoesClear()
        {
            bool clear = true;
            uut.RenderAirCraft(trackInput, clear);

            mockWriter.Received(1).ClearConsole();
        }

        [Test]
        public void RenderAirCraft_CallWithoutSecondParam_DoesClearDefault()
        {
            uut.RenderAirCraft(trackInput);
            mockWriter.Received(1).ClearConsole();
        }
        #endregion


        #region RenderConditionTests

        [Test]                       //Four is outputting all the text
        public void RenderCondition_RenderTwoTracks_ReceiveFourCalls()
        {
            var trackinput2 = new Track("234567", 1, 2, 3,date, 4, 5);

            uut.RenderCondition(trackInput, trackinput2, DateTime.Now);
            mockWriter.Received(4).Write(Arg.Any<string>());
        }

        [Test]                 //Three is outputting all the text correctly
        public void RenderCondition_RenderTwoTracks_ReceiveNoClear()
        {
            var trackinput2 = new Track("234567", 1, 2, 3,date, 4, 5);

            uut.RenderCondition(trackInput, trackinput2, DateTime.Now);
            mockWriter.Received(0).ClearConsole();
            
        }


        #endregion

    }



}
