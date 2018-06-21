using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ObserverPattern
{
    class Subject:ISubject
    {
        private List<Observer> observers = new List<Observer>();
        public Subject()
        {
            //LOOP: Generating Data
        }

        public void Subscribe(Observer observer)
        {
            observers.Add(observer);
        }
        
        public void Unsubscribe(Observer observer)
        {
            observers.Remove(observer);
        }
        
        public void Notify()
        {
            observers.ForEach(observer => ObserverUpdate(observer));
        }

        async Task<int> ObserverUpdate(Observer observer)
        {
            await Task.Run(() => observer.update());
            return 1;
        }
    }
    
    interface ISubject
    {
        void Subscribe();
        void Unsubscribe();
        void Notify();
    }
}