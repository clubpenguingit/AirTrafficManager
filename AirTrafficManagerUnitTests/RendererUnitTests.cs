using AirTrafficManager;
using NSubstitute;
using NUnit.Framework;

namespace AirTrafficManagerUnitTests
{
    [TestFixture]
    public class RendererUnitTests
    {
        private Renderer uut;
        private IWriter mockWriter;
        private Track trackInput;
        [SetUp]
        public void Setup()
        {
            mockWriter = Substitute.For<IWriter>();
            uut = new Renderer(mockWriter);
            trackInput = new Track(
                "ExpectedTag", 1000, 2000,
                4000, 5000, 300);
        }

        [Test]
        public void RenderAirCraft_RendersInputCorrectly()
        {
            //Act
            uut.RenderAirCraft(trackInput);
            //Assert
            mockWriter.Received(1).Write($"Tag: {trackInput.Tag}\n" +
                                         $"Coordinates: ({trackInput.XCoordinate} , {trackInput.YCoordinate})\n" +
                                         $"Altitude: {trackInput.Altitude}\n" +
                                         $"Velocity: {trackInput.Velocity}\n" +
                                         $"Compass course: {trackInput.CompassCourse}");
        }
    }
}