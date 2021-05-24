using System;
using System.Linq;
using System.Threading.Tasks;
using VoiceAssistant.Server.Messages;
using VoiceAssistant.Server.Server;
using VoiceAssistant.Server.SpeechToText;

namespace VoiceAssistant.Server
{
    public class Coordinator : ISpeechToTextListener, IWebSocketServerListener<Bag>
    {
        private readonly WebSocketServer<Bag> _server;
        private readonly SpeechToTextService _speechToTextService;

        private WebSocketClient<Bag> _activeClient = null;

        public Coordinator(
            WebSocketServer<Bag> server,
            SpeechToTextService speechToTextService)
        {
            _server = server ?? throw new ArgumentNullException(nameof(server));
            _speechToTextService = speechToTextService ?? throw new ArgumentNullException(nameof(speechToTextService));
        }

        public void ClientConnected(WebSocketClient<Bag> client)
        {
            client.Bag = new Bag();
        }

        public void ClientDisconnected(WebSocketClient<Bag> client)
        {
            if (client == _activeClient)
                _activeClient = null;
        }

        public async Task MessageReceived(WebSocketClient<Bag> client, string messageName, byte[] message)
        {
            switch (messageName)
            {
                case "activate":
                    _activeClient = client;
                    if (client.Bag.Commands == null)
                        await _server.Send(client, new CommandsNeededMessage());
                    else
                        _speechToTextService.ReloadPhrases(client.Bag.Commands);
                    break;

                case "setCommands":
                    var setCommands = JSON.Deserialize<SetCommandsMessage>(message);
                    client.Bag.Commands = setCommands.Commands;
                    _speechToTextService.ReloadPhrases(client.Bag.Commands);
                    break;
            }
        }

        public async Task PhraseRecognized(string phrase)
        {
            if (_activeClient != null && _activeClient.Bag.Commands.Contains(phrase))
                await _server.Send(_activeClient, new CommandRecognizedMessage() { Command = phrase });
        }
    }
}