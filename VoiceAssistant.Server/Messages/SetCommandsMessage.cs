namespace VoiceAssistant.Server.Messages
{
    public class SetCommandsMessage : IMessage
    {
        public string Name { get; } = "setCommands";
        public string[] Commands { get; set; }
    }
}