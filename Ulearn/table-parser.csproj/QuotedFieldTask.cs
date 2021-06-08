using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TableParser
{
    [TestFixture]
    public class QuotedFieldTaskTests
    {
        [TestCase("''", 0, "", 2)]
        [TestCase("'a'", 0, "a", 3)]
        [TestCase(@"' a ", 0, " a ", 4)]
        [TestCase("\"",0,"",1)]
        [TestCase(@"some_text ""QF \"""" other_text", 10, "QF \"", 7)]
        [TestCase(@"'a\' b'", 0, "a' b", 7)]
        [TestCase("\"\\\\\"", 0, "\\", 4)]
        [TestCase("'a\"\"'", 0, "a\"\"", 5)]
        [TestCase("\"a'\"", 0, "a'", 4)]
        [TestCase("'\\\"'", 0, "\"",4)]
        [TestCase("'",0,"",1)]
        public void Test(string line, int startIndex, string expectedValue, int expectedLength)
        {
            var actualToken = QuotedFieldTask.ReadQuotedField(line, startIndex);
            Assert.AreEqual(new Token(expectedValue, startIndex, expectedLength), actualToken);
        }

        [TestCase("'a'",0,'\'')]
        [TestCase(@"some_text ""QF \"""" other_text",10,'\"')]
        [TestCase("'a\\\' b'", 0,'\'')]
        public void TestOpenQuotes(string line, int startIndex, char expectedQuotes)
        {
            var actualQuotes = QuotedFieldTask.GetOpenQuotes(line, startIndex);
            Assert.AreEqual(expectedQuotes, (char)actualQuotes);
        }
    }

    class QuotedFieldTask
    {
        public static bool EndQuotes { get; set; } = false;
        public static int Length { get; set; } = 1;
        public enum TypeQuotes
        {
            Quotes = '\'',
            DoubleQuotes = '\"'
        }
        public static Token ReadQuotedField(string line, int startIndex)
        {
            var lineToken = GetSubstringToken(line, startIndex);
            TypeQuotes typeOpenQuotes = GetOpenQuotes(line, startIndex);
            var value = GetValueToken(lineToken, typeOpenQuotes);
            if (EndQuotes)
                Length++;
            EndQuotes = false;// For Tests
            var length = value.Length + Length;
            Length = 1;// For Tests
            return new Token(value, startIndex, length);
        }

        public static TypeQuotes GetOpenQuotes(string line, int startIndex)
        {
             var quotes = (line[startIndex] == '\'') ? TypeQuotes.Quotes : TypeQuotes.DoubleQuotes;
            return quotes;
        }

        public static string GetValueToken(string line, TypeQuotes typeQuotes)
        {
            var slash = '\\';
            var result = "";
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == slash)
                {
                    result += line[i + 1];
                    i++;
                    Length++;//Костыль
                }
                else if (IsQuotes(line[i], typeQuotes) && (i != 0))
                {
                    EndQuotes = true;
                    break;
                }
                else if ((!IsQuotes(line[i], typeQuotes)) && (line[i] != (char)typeQuotes))
                {
                    result += line[i];
                }
            }
            return result;
        }

        private static bool IsQuotes(char letter, TypeQuotes typeQuotes)
        {
            return ((typeQuotes == TypeQuotes.DoubleQuotes)
                ? letter == (char)TypeQuotes.DoubleQuotes : letter == (char)TypeQuotes.Quotes);
        }

        public static string GetSubstringToken(string line, int startIndex)
        {
            return line.Substring(startIndex);
        }
    }
}