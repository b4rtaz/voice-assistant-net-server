namespace VoiceAssistant.Server.Server
{
    public class WebSocketClient<TBag>
    {
        public string Ip { get; private set; }
        public TBag Bag { get; set; }

        public WebSocketClient(string ip)
        {
            Ip = ip;
        }
    }
}