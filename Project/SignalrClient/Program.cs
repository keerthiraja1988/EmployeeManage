using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalrClient
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            SignalrHandler();
            Console.ReadLine();
            var hubConnection = new HubConnection("http://localhost:8089");
            IHubProxy stockTickerHubProxy = hubConnection.CreateHubProxy("MyHub");
            stockTickerHubProxy.On("addMessage", stock => Console.WriteLine("Stock update for {0} new price {1}"));
            hubConnection.Start();
        }

        public static async void SignalrHandler()
        {
            var url = "http://localhost:8089";
            var querystringData = new Dictionary<string, string> { { "type", "WIN" } };
            var _hubConnection = new HubConnection(url);
            var MarcolinMainProxy = _hubConnection.CreateHubProxy("MyHub");
            MarcolinMainProxy.On<string, string>("addMessage", InvokeMethod);
            await _hubConnection.Start();
            Console.ReadLine();
        }

        private static void InvokeMethod(string type, string type1)
        {
            Console.WriteLine(String.Format("Recieved Message From Server On :{0}", System.DateTime.Now.ToString()));
            Console.WriteLine("Message Received");
        }
    }
}