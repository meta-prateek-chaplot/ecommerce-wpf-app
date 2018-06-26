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

        private Dictionary<string, ProductInfo> products = new Dictionary<string, ProductInfo>();
        private Object productsLock = new Object();

        public Observer(string name)
        {
            this.ObserverName = name;
        }

        public void Update(Dictionary<string, ProductInfo> products)
        {
            lock (productsLock)
            {
                //TODO
            }
        }
    }

    interface IObserver
    {
        void Update(Dictionary<string, ProductInfo> products);
    }
}
