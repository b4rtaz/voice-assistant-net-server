using NUnit.Framework;
using VoiceAssistant.Server.SpeechToText;

namespace VoiceAssistant.Server.Tests.SpeechToText
{
    public class GrammarParserTests
    {
        [Test]
        public void CreateTree__WHEN__PassedPhrases__THEN__ReturnsProperTree()
        {
            var grammar = GrammarParser.CreateTree(new string[] {
                "Put class",
                "Put function",
                "Create public function",
                "Test"
            });

            var put = grammar["Put"];
            Assert.Contains("class", put);
            Assert.Contains("function", put);

            var create = grammar["Create"];
            Assert.Contains("public function", create);

            Assert.IsNull(grammar["Test"]);
        }
    }
}