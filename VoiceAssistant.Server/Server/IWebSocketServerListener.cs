using System.Threading.Tasks;

namespace VoiceAssistant.Server.Server
{
    public interface IWebSocketServerListener<TBag>
    {
        void ClientConnected(WebSocketClient<TBag> client);
        void ClientDisconnected(WebSocketClient<TBag> client);
        Task MessageReceived(WebSocketClient<TBag> client, string messageName, byte[] message);
    }
}