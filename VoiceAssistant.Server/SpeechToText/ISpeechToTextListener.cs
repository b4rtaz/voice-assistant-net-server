using System.Threading.Tasks;

namespace VoiceAssistant.Server.SpeechToText
{
    public interface ISpeechToTextListener
    {
        Task PhraseRecognized(string phrase);
    }
}