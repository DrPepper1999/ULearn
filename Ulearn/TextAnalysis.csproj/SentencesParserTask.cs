using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TextAnalysis
{
    static class SentencesParserTask
    {
        public static List<List<string>> ParseSentences(string text)
        {
            var sentencesList = new List<List<string>>();
            Regex regexLine = new Regex(@"[.!?;:()]");
            Regex regexWord = new Regex(@"[^a-zA-Z'_]+");
            List<string> lineList = regexLine.Split(text).Where(s => s != String.Empty).ToList();
            foreach (var line in lineList)
            {
                sentencesList.Add(regexWord.Split(line).Where(s => s != String.Empty).Select(s => s.ToLower()).ToList());
            }
            return sentencesList.Where(x => x.Count != 0).ToList();
        }
    }
}