using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TextAnalysis
{
    static class FrequencyAnalysisTask
    {
        public static Dictionary<string, string> GetMostFrequentNextWords(List<List<string>> text)
        {
            Dictionary<string, Dictionary<string, int>> frequencyDictionary = GetFrequencyDictionary(text);
            var result = GetResultDictionary(frequencyDictionary);
            return result;
        }

        private static Dictionary<string, string> GetResultDictionary(Dictionary<string, Dictionary<string, int>> frequencyDictionary)
        {
            var result = new Dictionary<string, string>();
            foreach (var firstDic in frequencyDictionary)
            {
                int max = -1;
                string endKey = "";
                foreach (var secondDic in firstDic.Value)
                {
                    if (secondDic.Value > max)
                    {
                        max = secondDic.Value;
                        endKey = secondDic.Key;
                    }
                    if (secondDic.Value == max)
                    {
                        if (string.CompareOrdinal(secondDic.Key, endKey) < 0)
                            endKey = secondDic.Key;
                    }
                }
                result.Add(firstDic.Key, endKey);
            }
            return result;
        }

        private static Dictionary<string, Dictionary<string, int>> GetFrequencyDictionary(List<List<string>> text)
        {
            var frequencyDictionary = new Dictionary<string, Dictionary<string, int>>();
            GetBigram(text, frequencyDictionary);
            GetTrigram(text, frequencyDictionary);
            return frequencyDictionary;
        }

        private static void GetTrigram(List<List<string>> text, Dictionary<string, Dictionary<string, int>> frequencyDictionary)
        {
            foreach (var line in text)
            {
                for (int i = 0; i < line.Count - 2; i++)
                {
                    string startKey = GetKey(line[i], line[i + 1]);
                    string endKey = line[i + 2];
                    if (!frequencyDictionary.ContainsKey(startKey))
                    {
                        frequencyDictionary.Add(startKey, new Dictionary<string, int>());
                    }
                    if (!frequencyDictionary[startKey].ContainsKey(endKey))
                    {
                        frequencyDictionary[startKey].Add(endKey, 1);
                    }
                    else
                    {
                        frequencyDictionary[startKey][endKey] += 1;
                    }
                }
            }
        }

        private static void GetBigram(List<List<string>> text, Dictionary<string, Dictionary<string, int>> frequencyDictionary)
        {
            foreach (var line in text)
            {
                for (int i = 0; i < line.Count - 1; i++)
                {
                    string startKey = line[i];
                    string endKey = line[i + 1];
                    if (!frequencyDictionary.ContainsKey(startKey))
                    {
                        frequencyDictionary.Add(startKey, new Dictionary<string, int>());
                    }
                    if (!frequencyDictionary[startKey].ContainsKey(endKey))
                    {
                        frequencyDictionary[startKey].Add(endKey, 1);
                    }
                    else
                    {
                        frequencyDictionary[startKey][endKey] += 1;
                    }
                }
            }
        }
        private static string GetKey(string key1, string key2)
        {
            return string.Format("{0} {1}", key1, key2);
        }
    }
}