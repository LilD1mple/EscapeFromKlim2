namespace EFK2.Game.PostProcess.Interfaces
{
    public interface IPostProcessService
    {
        bool IsEnable { get; }

        void SetPostProcessEnable(bool enable);

        void Reset();
    }
}
