using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace TableParser
{
    [TestFixture]
    public class FieldParserTaskTests
    {
        public static void Test(string input, string[] expectedResult)
        {
            var actualResult = FieldsParserTask.ParseLine(input);
            Assert.AreEqual(expectedResult.Length, actualResult.Count);
            for (int i = 0; i < expectedResult.Length; ++i)
            {
                Assert.AreEqual(expectedResult[i], actualResult[i].Value);
            }
        }

        [TestCase("", new string[0])]
        [TestCase("hello  world", new[] { "hello", "world" })]
        [TestCase("hello world", new[] { "hello", "world" })]
        [TestCase("text", new[] { "text" })]
        [TestCase("'a b c'", new[] { "a b c" })]
        [TestCase("a b c", new[] { "a", "b", "c" })]
        [TestCase("a' '", new[] { "a", " " })]
        [TestCase("' 'a", new[] { " ", "a" })]
        [TestCase(" a ", new[] { "a" })]
        [TestCase("'a'", new[] { "a" })]
        [TestCase("\"a", new[] { "a" })]
        [TestCase("\"\\\"\"", new[] { "\"" })]
        [TestCase("\"\\'\"", new[] { "'" })]
        [TestCase("\"\\\\\"", new[] { "\\" })]
        [TestCase("'\\\"'", new[] { "\"" })]
        [TestCase("'\\\''", new[] { "\'" })]
        [TestCase("\"a\"''", new[] { "a", "" })]
        [TestCase("'a ", new[] { "a " })]
        public static void RunTests(string input, string[] expectedOutput)
        {
            Test(input, expectedOutput);
        }
    }

    public class FieldsParserTask
    {
        public static List<Token> ParseLine(string line)
        {
            List<Token> tokens = new List<Token>();
            char[] quotedChar = new char[] { '\"', '\'' };
            for (int i = 0; i < line.Length; i++)
            {
                if (quotedChar.Contains(line[i]))
                {
                    tokens.Add(ReadQuotedField(line, i));
                    i = tokens.Last().GetIndexNextToToken() - 1;
                }
                else if(line[i] != ' ')
                {
                    tokens.Add(ReadField(line, i));
                    i = tokens.Last().GetIndexNextToToken() - 1;
                }
            }
            return tokens;
        }

        private static Token ReadField(string line, int startIndex)
        {
            char[] stopChar = new char[] { ' ', '\'', '\"' };
            int i = startIndex;
            string value = "";
            for (; i < line.Length; i++)
            {
               if (stopChar.Contains(line[i]))
                    break;
                value += line[i];
            }
            return new Token(value, startIndex, value.Length);
        }

        public static Token ReadQuotedField(string line, int startIndex)
        {
            return QuotedFieldTask.ReadQuotedField(line, startIndex);
        }
    }
}