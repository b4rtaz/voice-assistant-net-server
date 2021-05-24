namespace VoiceAssistant.Server.Messages
{
    public class CommandsNeededMessage : IMessage
    {
        public string Name { get; } = "commandsNeeded";
    }
}