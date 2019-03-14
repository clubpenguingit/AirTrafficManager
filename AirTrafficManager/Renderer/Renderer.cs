using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace AirTrafficManager
{
    public class Renderer : IRenderer
    {
        private IWriter _writer;
        private ISeparationCondition _condition;

        public Renderer(IWriter writer   )//, ISeparationCondition sepcond)
        {
            _writer = writer;
           // _condition = sepcond;
            //Subscribe to conditionevent. 
        }
        public void RenderAirCrafts(List<Track> tracks, bool clear = true)
        {
            if(clear)
                _writer.ClearConsole(); // Skal fx ikke clear ved SeparationCondition

            foreach (var track in tracks)
            {
                _writer.Write($"Tag: {track.Tag}\n" +
                              $"Coordinates: ({track.XCoordinate} , {track.YCoordinate}) meters\n" +
                              $"Altitude: {track.Altitude} meters\n" +
                              $"Velocity: {track.Velocity} meters\n" +
                              $"Compass course: {track.CompassCourse}\n");
            } 
        }

        public void RenderCondition(Track track1, Track track2, DateTime dateTime)
        {
            var tracks = new List<Track>();
            tracks.Add(track1);
            tracks.Add(track2);

            _writer.Write($"\n***WARNING***\n" +
                          "Two aircrafts are flying too close!\n" +
                          $"Time of occurrence: {dateTime.ToUniversalTime()}\n" +
                          "The aircrafts are:\n ");

            RenderAirCrafts(tracks, false);


        }

        private void OnDataReadyInATMS(object sender, ATMSEventArgs e)
        {
            RenderAirCrafts(e.Tracks, true);
        }

        //private void OnSepCondition(object sender, TODO Todo)
        //{

        //}
    }
}