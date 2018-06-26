using System;
using System.Threading.Tasks;
using Grpc.Core;
using PubSub;
using System.Threading;

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
        }
    }

    class Program
    {
        const int Port = 50051;
        
        private static void GetData()
        {
            Subject subject = new Subject();

            subject.Subscribe(new Observer("Notifier Server"));
        }

        public static void Main(string[] args)
        {
            Thread startSub = new Thread(new ThreadStart(GetData));
            startSub.Start();
            
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
    }
}
