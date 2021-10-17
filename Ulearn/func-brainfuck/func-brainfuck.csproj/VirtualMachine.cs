using System;
using System.Collections.Generic;

namespace func.brainfuck
{
	public class VirtualMachine : IVirtualMachine
	{
		public string Instructions { get; }
		public int InstructionPointer { get; set; }
		public byte[] Memory { get; }
		public int MemoryPointer { get; set; }
		private Dictionary<char, Action<IVirtualMachine>> Comands { get; set; }

		public VirtualMachine(string program, int memorySize)
		{
			Instructions = program;
			InstructionPointer = 0;
			Memory = new byte[memorySize];
			MemoryPointer = 0;
			Comands = new Dictionary<char, Action<IVirtualMachine>>();
		}

		public void RegisterCommand(char symbol, Action<IVirtualMachine> execute)
		{
			Comands.Add(symbol, execute);
		}

		public void Run()
		{
            for (; InstructionPointer < Instructions.Length; InstructionPointer++)
            {
				var command = Instructions[InstructionPointer];
				if (Comands.ContainsKey(command))
					Comands[command](this);
			}
		}
	}
}