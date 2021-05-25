using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VoiceAssistant.Server.Logger;
using WatsonWebsocket;

namespace VoiceAssistant.Server.Server
{
    public class WebSocketServer<TBag>
    {
        private readonly string _host;
        private readonly int _port;
        private readonly ILogger _logger;

        private readonly List<IWebSocketServerListener<TBag>> _listeners = new List<IWebSocketServerListener<TBag>>();
        private readonly WatsonWsServer _server;

        private readonly List<WebSocketClient<TBag>> _clients = new List<WebSocketClient<TBag>>();

        public WebSocketServer(
            string host,
            int port,
            ILogger logger)
        {
            _host = host;
            _port = port;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _server = new WatsonWsServer(host, port);
            _server.ClientConnected += ClientConnected;
            _server.ClientDisconnected += ClientDisconnected;
            _server.MessageReceived += MessageReceived;
        }

        public Task Start()
        {
            _logger.Log($"Listening on {_host}:{_port}...");
            return _server.StartAsync();
        }

        public Task Send(WebSocketClient<TBag> client, object data)
        {
            var bytes = JSON.Serialize(data);
            return _server.SendAsync(client.Ip, bytes);
        }

        public void AddListener(IWebSocketServerListener<TBag> listener)
        {
            _listeners.Add(listener);
        }

        private void ClientConnected(object sender, ClientConnectedEventArgs e)
        {
            var client = new WebSocketClient<TBag>(e.IpPort);
            _clients.Add(client);
            _logger.Log($"Client connected: {e.IpPort}");
            _listeners.ForEach(l => l.ClientConnected(client));
        }

        private void ClientDisconnected(object sender, ClientDisconnectedEventArgs e)
        {
            var client = GetClient(e.IpPort);
            _clients.Remove(client);
            _logger.Log($"Client disconnected: {e.IpPort}");
            _listeners.ForEach(l => l.ClientConnected(client));
        }

        private async void MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            var client = GetClient(e.IpPort);
            var messageName = JSON.ReadRootProperty(e.Data, "name");
            _logger.Log($"Message received: {messageName} from {e.IpPort}");
            foreach (var listener in _listeners)
                await listener.MessageReceived(client, messageName, e.Data);
        }

        private WebSocketClient<TBag> GetClient(string clientIp)
        {
            return _clients.Find(c => c.Ip == clientIp)
                ?? throw new Exception($"Cannot find client by ip: {clientIp}.");
        }
    }
}