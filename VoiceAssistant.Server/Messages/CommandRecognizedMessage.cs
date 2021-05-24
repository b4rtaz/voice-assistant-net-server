namespace VoiceAssistant.Server.Messages
{
    public class CommandRecognizedMessage : IMessage
    {
        public string Name { get; } = "commandRecognized";
        public string Command { get; set; }
    }
}