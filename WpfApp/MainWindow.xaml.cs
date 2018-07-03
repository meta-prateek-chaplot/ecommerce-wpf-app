using Grpc.Core;
using PubSub;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace NotifierClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static ObservableCollection<KeyValuePair<int, double>> acVaules = new ObservableCollection<KeyValuePair<int, double>>();

        static int acCount = 1;

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
                    StringBuilder responseLog = new StringBuilder("Result:\n");

                    while (await responseStream.MoveNext())
                    {
                        DataReply dataReply = responseStream.Current;
                        timerTick(dataReply.ProductName, dataReply.ProductPrice);
                    }
                }
            }
        }

        public MainWindow()
        {
            Channel channel = new Channel("127.0.0.1:50051", ChannelCredentials.Insecure);
            var client = new NotifierClient(new Notifier.NotifierClient(channel));
            
            InitializeComponent();
            showChart();

            //DispatcherTimer timer = new DispatcherTimer();
            //timer.Interval = new TimeSpan(0, 0, 2);
            //timer.Tick += new EventHandler(timerTick);
            //timer.IsEnabled = true;

            client.GetData().Wait();
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
