using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SentencesParser
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = ParseSentences(@" Harry Potter and the Sorcerer's Stone




   For Jessica, who loves stories,
   For Anne, who loved them too;
            And for Di, who heard this one first.
         





             1.THE BOY WHO LIVED
         


            Mr.and Mrs.Dursley, of number four, Privet Drive, were proud to say that they were perfectly normal, thank you very much.They were the last people you'd expect to be involved in anything strange or mysterious, because they just didn't hold with such nonsense.
            Mr.Dursley was the director of a firm called Grunnings, which made drills.He was a big, beefy man with hardly any neck, although he did have a very large mustache.Mrs.Dursley was thin and blonde and had nearly twice the usual amount of neck, which came in very useful as she spent so much of her time craning over garden fences, spying on the neighbors.The Dursleys had a small son called Dudley and in their opinion there was no finer boy anywhere.");
            foreach (var item in result)
            {
                Console.WriteLine();
                foreach (var i in item)
                {
                    Console.Write("{0} ",i);
                }
            }
        }
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
