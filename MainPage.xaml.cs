using System;
using System.Threading;
using System.Windows;
using Client = CIAPI.Rpc.Client;

namespace PhoneApp4
{
    public partial class MainPage
    {
        private const string USERNAME = "xx921521";
        private const string PASSWORD = "welcome1";

        private static readonly Uri RPC_URI = new Uri("https://ciapipreprod.cityindextest9.co.uk/TradingApi");

        public Client RpcClient;
        private bool _stop;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void BuildClient()
        {
            //Dispatcher.BeginInvoke(() => listBox1.Items.Add("login started"));
            RpcClient = new Client(RPC_URI, "CI-WP7");
            RpcClient.BeginLogIn(USERNAME, PASSWORD, ar =>
            {
                try
                {
                    RpcClient.EndLogIn(ar);
                    //Dispatcher.BeginInvoke(() => listBox1.Items.Add("login completed"));
                }
                catch (Exception ex)
                {
                    Dispatcher.BeginInvoke(() => listBox1.Items.Add("exception caught: " + ex));
                    _stop = true;
                }
                if (_stop)
                {
                    _stop = false;
                    Dispatcher.BeginInvoke(() =>
                    {
                        button1.IsEnabled = true;
                        button2.IsEnabled = false;
                    });
                }
                else
                {
                    Thread.Sleep(100);
                    ThreadPool.QueueUserWorkItem(_ => BuildClient());
                }
            }, null);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                button1.IsEnabled = false;
                button2.IsEnabled = true;
                ThreadPool.QueueUserWorkItem(_ => BuildClient());
            });
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            button2.IsEnabled = false;
            _stop = true;
        }
    }
}
