using Microsoft.VisualStudio.TestTools.UnitTesting;
using Clones;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clones.Tests
{
    [TestClass()]
    public class StackTests
    {
       
        [TestMethod()]
        public void PushPopTest()
        {
            Stack<string> stack = new Stack<string>();
            var expected = "b";
            stack.Push("a");
            stack.Push("b");
            Assert.AreEqual(expected, stack.Pop());
        }
    }
}