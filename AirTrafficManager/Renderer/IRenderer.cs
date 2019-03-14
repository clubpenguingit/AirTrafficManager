using System;
using System.Collections.Generic;

namespace AirTrafficManager
{
    public interface IRenderer
    {
        void RenderAirCrafts(List<Track> tracks, bool clear = true);

        void RenderCondition(Track track1, Track track2, DateTime dateTime);

        
    }
}