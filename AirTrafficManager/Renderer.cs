using System;
using System.Runtime.InteropServices;

namespace AirTrafficManager
{
    public class Renderer : IRenderer
    {
        private IWriter _writer;

        public Renderer(IWriter writer)
        {
            _writer = writer;
        }
        public void RenderAirCraft(Track track, bool clear = true)
        {
            if(clear)
                Console.Clear(); // Skal fx ikke clear ved SeparationCondition

            _writer.Write($"Tag: {track.Tag}\n" +
                              $"Coordinates: ({track.XCoordinate} , {track.YCoordinate})\n" +
                              $"Altitude: {track.Altitude}\n" +
                              $"Velocity: {track.Velocity}\n" +
                              $"Compass course: {track.CompassCourse}");
        }

        public void RenderCondition(Track track1, Track track2, DateTime dateTime)
        {
            _writer.Write("***WARNING***\n" +
                          "Two aircrafts are flying too close: ");
            RenderAirCraft(track1, false);
            RenderAirCraft(track2, false);  


        }
    }
}