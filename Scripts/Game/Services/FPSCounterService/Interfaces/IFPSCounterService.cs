namespace EFK2.Game.FPSCounter.Interfaces
{
    public interface IFPSCounterService
    {
        int GetFPSCount();

        void SetFPSLimit(int count);
    }
}
