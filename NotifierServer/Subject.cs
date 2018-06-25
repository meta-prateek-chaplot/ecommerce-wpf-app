using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace NotifierServer
{
    class Subject : ISubject
    {
        public class ProductInfo
        {
            public string name;
            public int lastTimeStamp;
            public List<double> prices;

            public ProductInfo(string name, double price)
            {
                this.name = name;
                this.prices.Add(price);
                this.lastTimeStamp = -1;
            }
        }

        private List<Observer> observers = new List<Observer>();
        private Object observersLock = new Object();

        private Dictionary<string, ProductInfo> products = new Dictionary<string, ProductInfo>();
        private Object productsLock = new Object();

        public Subject()
        {
            products.Add("TV", new ProductInfo("TV", 9999));
            products.Add("Fridge", new ProductInfo("Fridge", 19999));
            products.Add("Mobile", new ProductInfo("Mobile", 8999));
            products.Add("AC", new ProductInfo("AC", 24999));
            products.Add("Bike", new ProductInfo("Bike", 34999));
        }

        private void LoopDataGeneration()
        {
            Random random = new Random();

            while (true)
            {
                lock (observersLock)
                {
                    if(observers.Count == 0)
                    {
                        break;
                    }
                }

                Thread.Sleep(5 * 1000);

                lock (productsLock)
                {
                    products["TV"] = random.Next(8999, 12999);
                    products["Fridge"] = random.Next(17999, 21999);
                    products["Mobile"] = random.Next(6999, 10999);
                    products["AC"] = random.Next(22999, 26999);
                    products["Bike"] = random.Next(32999, 36999);
                }

                Notify();
            }
        }

        public void Subscribe(Observer observer)
        {
            lock (observersLock)
            {
                observers.Add(observer);

                if(observers.Count == 1)
                {
                    Thread loopGeneration = new Thread(new ThreadStart(LoopDataGeneration));
                    loopGeneration.Start();
                }
            }
        }

        public void Unsubscribe(Observer observer)
        {
            lock (observersLock)
            {
                observers.Remove(observer);
            }
        }

        public void Notify()
        {
            lock (observersLock)
            {
                lock(productsLock)
                {
                    observers.ForEach(observer => ObserverUpdate(observer, products));
                }
            }
        }

        async Task<int> ObserverUpdate(Observer observer, Dictionary<string, int> products)
        {
            await Task.Run(() => observer.Update(products));
            return 0;
        }
    }

    interface ISubject
    {
        void Subscribe(Observer observer);
        void Unsubscribe(Observer observer);
        void Notify();
    }
}
