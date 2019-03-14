namespace AirTrafficManager.LoggingClasses
{
    public interface ILogger
    {
        void SepConditionOccured(object sender, SepCondEventArgs e);

    }
}