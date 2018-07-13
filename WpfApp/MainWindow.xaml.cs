// exception handling
// xaml chart coding
// current price textbox coding
// review xaml

using Grpc.Core;
using PubSub;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace NotifierClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<KeyValuePair<int, double>> acVaules = new ObservableCollection<KeyValuePair<int, double>>();
        public ObservableCollection<KeyValuePair<int, double>> bVaules = new ObservableCollection<KeyValuePair<int, double>>();
        public ObservableCollection<KeyValuePair<int, double>> tVaules = new ObservableCollection<KeyValuePair<int, double>>();

        int acCount = 0;
        int bCount = 0;
        int tCount = 0;

        Channel channel;
        Notifier.NotifierClient client;

        public async Task GetData(Notifier.NotifierClient client)
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

        public MainWindow()
        {
            channel = new Channel("127.0.0.1:50051", ChannelCredentials.Insecure);
            client = new Notifier.NotifierClient(channel);

            InitializeComponent();
            showChart();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await GetData(client);
            channel.ShutdownAsync().Wait();
        }

        public void timerTick(string name, double price)
        {
            if (name == "AC")
            {
                acVaules.Add(new KeyValuePair<int, double>(acCount, price));
                acCount++;
            }
            else if (name == "BIKE")
            {
                bVaules.Add(new KeyValuePair<int, double>(bCount, price));
                bCount++;
            }
            else if (name == "TV")
            {
                tVaules.Add(new KeyValuePair<int, double>(tCount, price));
                tCount++;
            }

            UpdateTextBox();
        }

        private void showChart()
        {
            lineSeries.DataContext = acVaules;
            lineSeries2.DataContext = bVaules;
            lineSeries3.DataContext = tVaules;
        }

        private void Tab_Loaded(object sender, RoutedEventArgs e)
        {
            textBox.Text = "0";
        }

        private void tc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateTextBox();
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        public void UpdateTextBox()
        {
            TabItem ti = tc.SelectedItem as TabItem;

            if (acVaules.Count == 0)
            {
                return;
            }

            if (ti.Header.ToString() == "AC")
            {
                textBox.Text = acVaules[acVaules.Count - 1].Value.ToString();
            }
            else if (ti.Header.ToString() == "BIKE")
            {
                textBox.Text = bVaules[bVaules.Count - 1].Value.ToString();
            }
            else if (ti.Header.ToString() == "TV")
            {
                textBox.Text = tVaules[tVaules.Count - 1].Value.ToString();
            }
        }
    }
}
