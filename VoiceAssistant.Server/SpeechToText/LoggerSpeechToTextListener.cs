using VoiceAssistant.Server.Logger;
using System;
using System.Threading.Tasks;

namespace VoiceAssistant.Server.SpeechToText
{
    public class LoggerSpeechToTextListener : ISpeechToTextListener
    {
        private readonly ILogger _logger;

        public LoggerSpeechToTextListener(
            ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task PhraseRecognized(string text)
        {
            _logger.Log($"Recognized: {text}");
            return Task.CompletedTask;
        }
    }
}