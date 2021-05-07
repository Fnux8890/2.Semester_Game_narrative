namespace GameSystems.CustomEventSystems.ObserverPattern
{
    public interface ISubject
    {
        public void Add(IObserver observer);
        public void Remove(IObserver observer);
        public void Notify();
    }
}