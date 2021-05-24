using System;
using VoiceAssistant.Server.Logger;
using VoiceAssistant.Server.Server;
using VoiceAssistant.Server.SpeechToText;

namespace VoiceAssistant.Server
{
    public class Program
    {
        public static void Main()
        {
            var logger = new ConsoleLogger();

            var server = new WebSocketServer<Bag>("localhost", 9999, logger);

            var speechToTextService = new SpeechToTextService();
            var coordinator = new Coordinator(server, speechToTextService);

            speechToTextService.AddListener(new LoggerSpeechToTextListener(logger));
            speechToTextService.AddListener(coordinator);
            speechToTextService.Start();

            server.AddListener(coordinator);

            server.Start().Wait();

            Console.ReadKey();
        }
    }
}