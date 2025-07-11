namespace EFK2.Game.ScreenService.Interfaces
{
    public interface IScreenService
    {
        int ResolutionIndex { get; }

        bool FullScreen { get; }

        void SetScreenState(bool state);

        void SetScreenResolution(int index);

        void ResetScreenState(bool defaultState);

        void ResetScreenResolution(int defaultResolutionIndex);
    }
}
