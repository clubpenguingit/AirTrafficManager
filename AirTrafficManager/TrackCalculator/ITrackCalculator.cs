namespace AirTrafficManager
{
    public interface ITrackCalculator
    {
        double CalculateVelocity(int x1, int x2, int y1, int y2, int z1, int z2, double time1, double time2);
        double CalculateCourse(int x1, int x2, int y1, int y2);

    }
}