using System;

namespace ObserverPattern
{
    class Observer:IObserver
    {
        public string ObserverName { get; private set; }
        public Observer(string name)
        {
            this.ObserverName = name;
        }
        public void Update(){

        }
    }
    interface IObserver
    {
        void Update();
    }
}