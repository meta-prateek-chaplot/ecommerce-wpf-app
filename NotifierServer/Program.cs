using System;
using System.Threading.Tasks;
using Grpc.Core;
using PubSub;
using System.Threading;
using System.Collections.Generic;

namespace NotifierServer
{
    class NotifierImpl : Notifier.NotifierBase
    {
        // Server side handler of the SayHello RPC
        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply { Message = "Hello " + request.Name });
        }

        public override async Task Data(DataRequest request, IServerStreamWriter<DataReply> replyStream, ServerCallContext context)
        {
            //TODO
            // send name, price, and
            // update timestamp

            //lock (Observer.productsLock)
            //{
            //    foreach (string product in Observer.products.Keys)
            //    {
            //        //Console.WriteLine("product: " + product);
            //    }
            //}
        }
    }

    class Program
    {
        const int Port = 50051;
        
        public static void Main(string[] args)
        {
            // Starting DataClass
            DataClass obj = new DataClass();

            Server server = new Server
            {
                Services = { Notifier.BindService(new NotifierImpl()) },
                Ports = { new ServerPort("localhost", Port, ServerCredentials.Insecure) }
            };
            server.Start();

            Console.WriteLine("Greeter server listening on port " + Port);
            Console.WriteLine("Press any key to stop the server...");
            Console.ReadKey();

            server.ShutdownAsync().Wait();
        }

        static void printData()
        {
            while(true)
            {
                Thread.Sleep(5 * 1000);

                lock (DataClass.productLock)
                {
                    foreach (KeyValuePair<int, Product> product in DataClass.products)
                    {
                        Console.WriteLine(product.Value.name + ": " + string.Join(", ", product.Value.price));
                    }

                }
            }
        }
    }
}
