namespace DriftRacer.Input
{
    public interface IInputProvider
    {
        float GetSteering();
        bool IsHandbrakePressed();
        bool IsBrakePressed();
    }
}
