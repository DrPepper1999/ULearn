using System;
using System.Collections.Generic;
using System.Linq;

namespace func.brainfuck
{
	public class BrainfuckLoopCommands
	{
       
        public static void RegisterTo(IVirtualMachine vm)
		{
            var brackPairs = GetBracketsPairs(vm.Instructions);

            vm.RegisterCommand('[', b => {
                if (b.Memory[b.MemoryPointer] == 0)
                    b.InstructionPointer = brackPairs.Item2[b.InstructionPointer];
            });
			vm.RegisterCommand(']', b => {
                if (b.Memory[b.MemoryPointer] != 0)
                    b.InstructionPointer = brackPairs.Item1[b.InstructionPointer];
            });
		}

        public static Tuple<Dictionary<int, int>, Dictionary<int, int>> GetBracketsPairs(string str)
        {
            var brackPairsOpen = new Dictionary<int, int>();
            var brackPairsClose = new Dictionary<int, int>();
            var pairs = new Dictionary<char, char>();
            pairs.Add('[', ']');
            var stack = new Stack<Tuple<char,int>>();

            for (int i = 0; i < str.Length; i++)
            {
                var symbol = str[i];
                if (pairs.ContainsKey(symbol)) stack.Push(Tuple.Create(symbol,i));
                else if (pairs.ContainsValue(symbol))
                {
                    var brack = stack.Pop();
                    if (pairs[brack.Item1] == symbol)
                    {
                        brackPairsOpen.Add(i, brack.Item2);
                        brackPairsClose.Add(brack.Item2, i);
                    }
                }
            }
            return Tuple.Create(brackPairsOpen, brackPairsClose);

        }
    }
}