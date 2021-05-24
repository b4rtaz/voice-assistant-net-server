using System.Collections.Generic;
using System.Globalization;
using System.Speech.Recognition;

namespace VoiceAssistant.Server.SpeechToText
{
    public class SpeechToTextService
    {
        private readonly List<ISpeechToTextListener> _listeners = new List<ISpeechToTextListener>();
        private readonly SpeechRecognitionEngine _engine;
        private readonly object _engineLock = new object();

        private bool _isListening = false;

        public SpeechToTextService()
        {
            _engine = new SpeechRecognitionEngine(new CultureInfo("en-US"));
            _engine.SpeechRecognized += SpeechRecognized;
        }

        public void AddListener(ISpeechToTextListener listener)
        {
            _listeners.Add(listener);
        }

        public void Start()
        {
            _engine.SetInputToDefaultAudioDevice();
        }

        public void ReloadPhrases(string[] phrases)
        {
            lock (_engineLock)
            {
                if (_isListening)
                    _engine.UnloadAllGrammars();

                var grammar = GrammarParser.Parse(phrases);
                _engine.LoadGrammar(grammar);

                // https://stackoverflow.com/a/18841928
                var noiseCatcher = new DictationGrammar("grammar:dictation#pronunciation");
                _engine.LoadGrammar(noiseCatcher);

                if (!_isListening)
                {
                    _engine.RecognizeAsync(RecognizeMode.Multiple);
                    _isListening = true;
                }
                else
                {
                    _engine.RequestRecognizerUpdate();
                }
            }
        }

        private async void SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            foreach (var listener in _listeners)
                await listener.PhraseRecognized(e.Result.Text);
        }
    }
}