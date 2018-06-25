using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotifierServer
{
    class Observer:IObserver
    {
        public string ObserverName { get; private set; }

        private Dictionary<string, int> products = new Dictionary<string, int>();
        private Object productsLock = new Object();

        public Observer(string name)
        {
            this.ObserverName = name;
        }

        public void Update(Dictionary<string, int> products)
        {
            lock (productsLock)
            {
                this.products = products;
            }
        }
    }

    interface IObserver
    {
        void Update(Dictionary<string, int> products);
    }
}
