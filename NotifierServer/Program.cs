using System;
using System.Threading.Tasks;
using Grpc.Core;
using PubSub;
using System.Collections.Generic;

namespace NotifierServer
{
    class NotifierImpl : Notifier.NotifierBase
    {
        int secToSendData = 10 * 1000;

        Dictionary<int, int> productTimeStamp = null;

        private void SetTimeStamp()
        {
            lock (DataClass.productLock)
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
        }

        private bool priceListUpdate()
        {
            lock (DataClass.productLock)
            {
                foreach (KeyValuePair<int, Product> product in DataClass.products)
                {
                    if (productTimeStamp[product.Key] != product.Value.priceList.Count)
                    {
                        //Console.WriteLine("True returned!");
                        return true;
                    }
                }
            }

            return false;
        }

        // while(true){}??, first see functioning of routeguide sample wpf application
        // System.Threading.Thread.Sleep(4 * 1000);
        public override async Task Data(DataRequest request, IServerStreamWriter<DataReply> replyStream, ServerCallContext context)
        {
            SetTimeStamp();
            while (true)
            {
                System.Threading.Thread.Sleep(secToSendData);

                if (priceListUpdate())
                {
                    List<DataReply> responseList = new List<DataReply>();

                    lock (DataClass.productLock)
                    {
                        foreach (KeyValuePair<int, Product> product in DataClass.products)
                        {
                            string prodName = product.Value.name;
                            int key = product.Key;

                            for (int index = productTimeStamp[key]; index < product.Value.priceList.Count; index++)
                            {
                                responseList.Add(new DataReply { ProductName = prodName, ProductPrice = product.Value.priceList[index] });
                            }

                            productTimeStamp[key] = product.Value.priceList.Count;
                        }
                    }

                    foreach (DataReply response in responseList)
                    {
                        await replyStream.WriteAsync(response);
                    }
                    responseList.Clear();
                }
            }
        }
    }

    //ISSUE: server not closing...
    // system.threading.thread.currentthread.abort();
    class Program
    {
        const int Port = 50051;

        // exception handling
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
