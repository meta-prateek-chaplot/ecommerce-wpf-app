using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using PubSub;

namespace NotifierClient
{
    class Program
    {
        public class NotifierClient
        {
            readonly Notifier.NotifierClient client;

            public NotifierClient(Notifier.NotifierClient client)
            {
                this.client = client;
            }

            public async Task Data()
            {
                DataRequest request = new DataRequest { };

                using (var call = client.Data(request))
                {
                    var responseStream = call.ResponseStream;
                    StringBuilder responseLog = new StringBuilder("Result: ");

                    while(await responseStream.MoveNext())
                    {
                        DataReply dataReply = responseStream.Current;
                        responseLog.Append(dataReply);
                    }

                    Console.WriteLine(responseLog.ToString());
                }
            }
        }

        // exception handling
        static void Main(string[] args)
        {
            Channel channel = new Channel("127.0.0.1:50051", ChannelCredentials.Insecure);

            var client = new NotifierClient(new Notifier.NotifierClient(channel));

            client.Data().Wait();

            channel.ShutdownAsync().Wait();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
