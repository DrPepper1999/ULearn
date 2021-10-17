using Microsoft.VisualStudio.TestTools.UnitTesting;
using func.brainfuck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace func.brainfuck.Tests
{
    [TestClass()]
    public class BrainfuckLoopCommandsTests
    {
        [TestMethod()]
        public void GetBracketsPairsTest()
        {
            var actual = BrainfuckLoopCommands.GetBracketsPairs("[[][]]");
            var expected = new Dictionary<int, int>();
            expected.Add(2, 1);
            expected.Add(4, 3);
            expected.Add(5, 0);
            CollectionAssert.AreEqual(actual.Item1, expected);
        }
    }
}