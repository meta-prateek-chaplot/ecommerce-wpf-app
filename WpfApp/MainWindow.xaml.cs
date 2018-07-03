using Grpc.Core;
using PubSub;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace NotifierClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static ObservableCollection<KeyValuePair<int, double>> acVaules = new ObservableCollection<KeyValuePair<int, double>>();
        static int acCount = 0;

        public class NotifierClient
        {
            readonly Notifier.NotifierClient client;

            public NotifierClient(Notifier.NotifierClient client)
            {
                this.client = client;
            }

            public async Task GetData()
            {
                DataRequest request = new DataRequest { };

                using (var call = client.Data(request))
                {
                    var responseStream = call.ResponseStream;

                    while (await responseStream.MoveNext())
                    {
                        DataReply dataReply = responseStream.Current;

                        timerTick(dataReply.ProductName, dataReply.ProductPrice);
                    }
                }
            }
        }

        Channel channel;
        NotifierClient client;

        public MainWindow()
        {
            channel = new Channel("127.0.0.1:50051", ChannelCredentials.Insecure);
            client = new NotifierClient(new Notifier.NotifierClient(channel));

            InitializeComponent();
            showChart();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await client.GetData();
            channel.ShutdownAsync().Wait();
        }

        public static void timerTick(string name, double price)
        {
            if (name == "AC")
            {
                acVaules.Add(new KeyValuePair<int, double>(acCount, price));
                acCount += 1;
            }
        }

        private void showChart()
        {
            lineSeries.DataContext = acVaules;
        }
    }
}
