namespace EFK2.Inputs.Interfaces
{
    public interface IMouseSensivityService
    {
        float MouseSensivity { get; }

        void SetMouseSensivity(float mouseSensivity);

        void ResetSensivity(float defaultSensivity);
    }
}
