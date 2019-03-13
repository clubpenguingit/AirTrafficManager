using System;

namespace AirTrafficManager
{
    public interface IRenderer
    {
        void RenderAirCraft(Track track, bool clear = true);

        void RenderCondition(Track track1, Track track2, DateTime dateTime);
    }
}