namespace EFK2.Events.Interfaces
{
    public interface IEventReceiver<T> : IBaseEventReceiver where T : struct, IEvent
    {
        void OnEvent(T @event);
    }
}
