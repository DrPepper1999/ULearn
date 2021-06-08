using System.Collections.Generic;
using System.Linq;

namespace TextAnalysis
{
    static class TextGeneratorTask
    {
        public static string ContinuePhrase( Dictionary<string, string> nextWords,string phraseBeginning
            ,int wordsCount)
        {
            for (int i = 0; i < wordsCount; i++)
            {
                bool flag = true;
                if (phraseBeginning.Contains(" "))
                {
                    var lastTwoWord = GetLast2Words(phraseBeginning).Trim();
                    if (nextWords.ContainsKey(lastTwoWord))
                    {
                        phraseBeginning = string.Join(" ", phraseBeginning, nextWords[lastTwoWord]);
                        flag = false;
                    }
                }
                if(flag)
                {
                    string lastWord = GetLastWord(phraseBeginning).Trim();
                    if (nextWords.ContainsKey(lastWord))
                    {
                        phraseBeginning = string.Join(" ", phraseBeginning, nextWords[lastWord]);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return phraseBeginning;
        }

        private static string GetLastWord(string phraseBeginning)
        {
            if (!phraseBeginning.Contains(" "))
                return phraseBeginning;
            return phraseBeginning.Substring(phraseBeginning.LastIndexOf(' '));
        }

        public static string GetLast2Words(string mainStr)
        {
            if (mainStr.Split(' ').Length == 2)
                return mainStr;
            int position = mainStr.LastIndexOf(' ', mainStr.LastIndexOf(' ') - 1);
            return position >= 0 ? mainStr.Substring(position) : null;
        }
    }
}