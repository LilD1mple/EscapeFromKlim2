namespace EFK2.Inputs.Interfaces
{
    public interface IMouseInputService
    {
        IMouseSensivityService MouseSensivityService { get; }

        float GetMouseLookX();

        float GetMouseLookY();

        void SetCursorState(bool state);
    }
}