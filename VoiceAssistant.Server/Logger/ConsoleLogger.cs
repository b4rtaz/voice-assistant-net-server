using System;

namespace VoiceAssistant.Server.Logger
{
    public class ConsoleLogger : ILogger
    {
        public void Log(string log)
        {
            var now = DateTime.Now.ToLongTimeString();
            Console.WriteLine($"{now} {log}");
        }
    }
}