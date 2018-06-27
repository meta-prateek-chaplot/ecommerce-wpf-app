using System;
using System.Collections.Generic;
using System.Threading;

namespace NotifierServer
{
    public struct Product {
        public string name;
        public List<double> price;
        public int lastTimeStamp;

        public Product(string name)
        {
            this.name = name;
            price = new List<double>();
            lastTimeStamp = 0;
        }

        public void setLastTimeStamp(int lastTimeStamp)
        {
            this.lastTimeStamp = lastTimeStamp;
        }
    }

    class DataClass
    {
        public static Dictionary<int, Product> products = new Dictionary<int, Product>();
        public static Object productLock = new Object();

        public DataClass()
        {
            lock (productLock)
            {
                products.Add(1, new Product("Tv"));
                products.Add(2, new Product("Ac"));
                products.Add(3, new Product("Bike"));
            }

            Thread th = new Thread(new ThreadStart(generateLoop));
            th.Start();
        }

        public void generateLoop()
        {
            Random random = new Random();

            while (true)
            {
                lock (productLock)
                {
                    Thread.Sleep(5 * 1000);

                    products[1].price.Add(random.Next(8999, 10999));
                    products[2].price.Add(random.Next(22499, 26499));
                    products[3].price.Add(random.Next(27999, 31999));

                    random.NextDouble();
                }
            }
        }
    }
}
