using System;
using System.Collections.Generic;
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
        int secToGenerateData = 2 * 1000;

        public static Dictionary<int, Product> products = new Dictionary<int, Product>();
        
        public DataClass()
        {
            products.Add(0, new Product("TV"));
            products.Add(1, new Product("AC"));
            products.Add(2, new Product("BIKE"));

            Timer t = new Timer();
            t.Elapsed += new ElapsedEventHandler(GenerateLoop);
            t.Interval = secToGenerateData;
            t.Enabled = true;
        }

        public void GenerateLoop(object source, ElapsedEventArgs e)
        {
            Random random = new Random();

            products[0].priceList.Add(random.Next(8999, 10999));
            products[1].priceList.Add(random.Next(22499, 26499));
            products[2].priceList.Add(random.Next(27999, 31999));
        }
    }
}
