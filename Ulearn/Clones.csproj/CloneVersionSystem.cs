using System;
using System.Collections.Generic;
using System.Linq;

namespace Clones
{
	public class CloneVersionSystem : ICloneVersionSystem
	{
		private readonly List<Clone> clones = new List<Clone>();

        public CloneVersionSystem()
        {
			clones = new List<Clone>() { new Clone() };
        }

        public string Execute(string query)
		{	
			var command = query.Split(' ');
			var cloneNum = Convert.ToInt32(command[1]) - 1;

			switch (command[0])
            {
				case "learn":
					var programNum = Convert.ToInt32(command[2]);
					clones[cloneNum].Learn(programNum);
					break;
				case "rollback":
					clones[cloneNum].Rollback();
					break;
				case "relearn":
					clones[cloneNum].Relearn();
					break;
				case "clone":
					clones.Add(new Clone(clones[cloneNum]));
					break;
				case "check":
					return clones[cloneNum].Check();

			}
            return null;
        }
    }
    public class Clone
	{
		private readonly Stack<int> assimilatedPrograms;
		private readonly Stack<int> canceledProgams;

		public Clone()
		{
			assimilatedPrograms = new Stack<int>();
			canceledProgams = new Stack<int>();
		}

        public Clone(Clone anotherClone)
        {
            assimilatedPrograms = new Stack<int>(anotherClone.assimilatedPrograms);
            canceledProgams = new Stack<int>(anotherClone.canceledProgams);
        }

        public void Learn(int program)
		{
			canceledProgams.Clear();
			assimilatedPrograms.Push(program);
		}

		public void Rollback()
        {
			var lastProgram = assimilatedPrograms.Pop();
			canceledProgams.Push(lastProgram);
        }

		public void Relearn()
        {
			var lastCanceled = canceledProgams.Pop();
			assimilatedPrograms.Push(lastCanceled);
        }

		public string Check()
        {
			if (assimilatedPrograms.isEmpty())
				return "basic";
			else return assimilatedPrograms.Peek().ToString();
		}
	}

	public class StackItem<T>
    {
        public T Value { get; set; }
		public StackItem<T> Previous { get; set; }

		public StackItem() { }
		public StackItem(T value, StackItem<T> previous)
		{
			Value = value;
			Previous = previous;
		} 
	}
	public class Stack<T>
    {
		StackItem<T> tail;

		public Stack() { }

		public Stack(Stack<T> stack)
        {
			tail = stack.tail;
        }

        public void Push(T value)
        {
			if (tail == null)
				tail = new StackItem<T> { Value = value, Previous = null };
			else
            {
				tail = new StackItem<T> { Value = value, Previous = tail };
            }
        }

		public T Pop()
        {
			if (tail == null) throw new InvalidOperationException();
			var result = tail.Value;
			tail = tail.Previous;
			return result;
        }

		public T Peek()
        {
			return tail.Value;
        }

		public void Clear()
        {
			tail = null;
        }

		public bool isEmpty()
        {
			return tail == null;
        }
    }
}
