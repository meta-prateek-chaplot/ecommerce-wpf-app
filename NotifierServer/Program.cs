using System;
using System.Threading.Tasks;
using Grpc.Core;
using PubSub;
using System.Collections.Generic;

namespace NotifierServer
{
    class NotifierImpl : Notifier.NotifierBase
    {
        public override async Task Data(DataRequest request, IServerStreamWriter<DataReply> replyStream, ServerCallContext context)
        {
            List<Product> localProducts = null;
            List<int> timeStamp = new List<int>();
            
            // while(true){}??, first see functioning of routeguide sample wpf application

            lock (DataClass.productLock)
            {
                if(localProducts is null)
                {
                    localProducts = new List<Product>();

                    foreach (KeyValuePair<int, Product> product in DataClass.products)
                    {
                        localProducts.Add(product.Value);
                    }
                }
                else
                {
                    foreach(KeyValuePair<int, Product> product in DataClass.products)
                    {
                        List<double> prices = product.Value.price;
                        int count = localProducts[product.Key].lastTimeStamp;
                        prices.RemoveRange(0, count);

                        localProducts[product.Key].price.AddRange(prices);
                    }
                }
            }

            foreach(Product product in localProducts)
            {
                string prodName = product.name;
                timeStamp.Add(product.price.Count);
                
                for(int index = product.lastTimeStamp; index < product.price.Count; index++)
                {
                    DataReply response = new DataReply { ProductName = prodName, ProductPrice = product.price[index] };
                    await replyStream.WriteAsync(response);
                }

                product.setLastTimeStamp(product.price.Count);
            }
        }
    }

    class Program
    {
        const int Port = 50051;
        
        public static void Main(string[] args)
        {
            // Starting DataClass
            DataClass obj = new DataClass();

            // Starting gRPC Server
            Server server = new Server
            {
                Services = { Notifier.BindService(new NotifierImpl()) },
                Ports = { new ServerPort("localhost", Port, ServerCredentials.Insecure) }
            };
            server.Start();

            Console.WriteLine("Notifier server listening on port " + Port);
            Console.WriteLine("Press any key to stop the server...");
            Console.ReadKey();

            server.ShutdownAsync().Wait();
        }
    }
}
