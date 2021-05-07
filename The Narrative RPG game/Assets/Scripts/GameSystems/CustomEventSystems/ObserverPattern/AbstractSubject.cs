using System.Collections.Generic;

namespace GameSystems.CustomEventSystems.ObserverPattern
{
    public abstract class AbstractSubject : ISubject
    {
        public List<AbstractObserver> _observers;

        protected AbstractSubject(AbstractObserver observer)
        {
            
        }

        public abstract void Add(IObserver observer);

        public abstract void Remove(IObserver observer);

        public abstract void Notify();
    }
}