using System;
using System.Threading.Tasks;
using Grpc.Core;
using PubSub;
using System.Collections.Generic;

namespace NotifierServer
{
    class NotifierImpl : Notifier.NotifierBase
    {
        Dictionary<int, int> productTimeStamp = null;

        private void InitializeTimeStamp()
        {
            if (productTimeStamp is null)
            {
                productTimeStamp = new Dictionary<int, int>();
                foreach (KeyValuePair<int, Product> product in DataClass.products)
                {
                    productTimeStamp.Add(product.Key, 0);
                }
            }
        }

        private bool priceListUpdate()
        {
            if (productTimeStamp[0] < DataClass.products[0].priceList.Count)
            {
                return true;
            }

            return false;
        }

        public override async Task Data(DataRequest request, IServerStreamWriter<DataReply> replyStream, ServerCallContext context)
        {
            InitializeTimeStamp();

            while (true)
            {
                if (priceListUpdate())
                {
                    foreach (KeyValuePair<int, Product> product in DataClass.products)
                    {
                        string prodName = product.Value.name;
                        int key = product.Key;
                        for (int index = productTimeStamp[key], count = product.Value.priceList.Count; index < count; index++)
                        {
                            await replyStream.WriteAsync(new DataReply { ProductName = prodName, ProductPrice = product.Value.priceList[index] });
                        }

                        productTimeStamp[key] = product.Value.priceList.Count;
                    }
                }
            }
        }
    }

    class Program
    {
        const int Port = 50051;

        static void Main(string[] args)
        {
            // Starting DataClass
            new DataClass();

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
