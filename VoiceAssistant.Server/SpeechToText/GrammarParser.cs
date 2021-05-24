using System;
using System.Collections.Generic;
using System.Speech.Recognition;

namespace VoiceAssistant.Server.SpeechToText
{
    public class GrammarParser
    {
        public static Dictionary<string, List<string>> CreateTree(string[] phrases)
        {
            var tree = new Dictionary<string, List<string>>();

            foreach (var phrase in phrases)
            {
                var parts = phrase.Split(" ", 2);
                if (parts.Length == 2)
                {
                    var verb = parts[0];
                    if (!tree.ContainsKey(verb))
                        tree.Add(verb, new List<string>());
                    tree[verb].Add(parts[1]);
                }
                else if (!tree.ContainsKey(phrase))
                {
                    tree.Add(phrase, null);
                }
            }

            return tree;
        }

        public static Grammar Parse(string[] phrases)
        {
            if (phrases == null) throw new ArgumentNullException(nameof(phrases));

            var tree = CreateTree(phrases);

            var finalChoice = new Choices();
            foreach (var item in tree)
            {
                if (item.Value == null)
                {
                    finalChoice.Add(item.Key);
                }
                else
                {
                    var builder = new GrammarBuilder();
                    builder.Append(item.Key);
                    builder.Append(new Choices(item.Value.ToArray()));
                    finalChoice.Add(builder);
                }
            }

            var finalBuilder = new GrammarBuilder(finalChoice);
            return new Grammar(finalBuilder);
        }
    }
}