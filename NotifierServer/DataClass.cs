using System;
using System.Collections.Generic;
using System.Threading;
using System.Timers;

namespace NotifierServer
{
    public struct Product {
        public string name;
        public List<double> priceList;

        public Product(string name)
        {
            this.name = name;
            priceList = new List<double>();
        }
    }

    class DataClass
    {
        int secToGenerateData = 10 * 1000;      // time req for price data to be generated

        public static Dictionary<int, Product> products = new Dictionary<int, Product>();
        public static Object productLock = new Object();

        public DataClass()
        {
            lock (productLock)
            {
                products.Add(0, new Product("TV"));
                products.Add(1, new Product("AC"));
                products.Add(2, new Product("BIKE"));
            }

            System.Timers.Timer t = new System.Timers.Timer();
            t.Elapsed += new ElapsedEventHandler(GenerateLoop);
            t.Interval = secToGenerateData;
            t.Enabled = true;
        }

        public void GenerateLoop(object source, ElapsedEventArgs e)
        {
            Random random = new Random();

            while (true)
            {
                Thread.Sleep(secToGenerateData);

                lock (productLock)
                {
                    products[0].priceList.Add(random.Next(8999, 10999));
                    products[1].priceList.Add(random.Next(22499, 26499));
                    products[2].priceList.Add(random.Next(27999, 31999));
                }
            }
        }
    }
}
