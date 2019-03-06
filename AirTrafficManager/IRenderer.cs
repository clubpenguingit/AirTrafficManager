using System;

namespace AirTrafficManager
{
    interface IRenderer
    {
        void RenderAirCraft(Track track);

        void RenderCondition(Track track1, Track track2, DateTime dateTime);
    }
}