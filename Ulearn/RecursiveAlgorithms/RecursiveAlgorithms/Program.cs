using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace RecursiveAlgorithms
{
	public static class Program
	{
		public static void Main()
		{
			MakePermutations(new int[4], 0);
		}

		public static int[] FindBestCheckpointsOrder(Point[] checkpoints)
		{
			var bestOrder = MakeTrivialPermutation(checkpoints.Length);

			return bestOrder;
		}


		public static void MakePermutations(int[] path, int position)
		{
			if (position == path.Length)
			{
				foreach (var item in path)
				{
					Console.Write($"{item} ");
				}
				Console.WriteLine();
				return;
			}

			for (int i = 0; i < path.Length; i++)
			{
				var index = Array.IndexOf(path, i, 0, position);
				if (index != -1)
					continue;
				path[position] = i;
				MakePermutations(path, position + 1);
			}
		}

		private static int[] MakeTrivialPermutation(int size)
		{
			var bestOrder = new int[size];
			for (int i = 0; i < bestOrder.Length; i++)
				bestOrder[i] = i;
			return bestOrder;
		}
	}
}
