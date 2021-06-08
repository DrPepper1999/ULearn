using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace RoutePlanning
{
	public static class PathFinderTask
	{
		public static int[] BestOrder;
		public static int[] FindBestCheckpointsOrder(Point[] checkpoints)
		{
			BestOrder = MakeTrivialPermutation(checkpoints.Length);
            int[] order = new int[checkpoints.Length];
            MakePermutations(checkpoints, order, 1);
			return BestOrder;
		}


		public static void MakePermutations(Point[] checkpoints, int[] path, int position)
        {
            var currentOrder = new int[position];
            Array.Copy(path, currentOrder, position);
            var pathLength = PointExtensions.GetPathLength(checkpoints, currentOrder);
            var bestOrderLength = PointExtensions.GetPathLength(checkpoints, BestOrder);
            if (pathLength < bestOrderLength)
            {
                if (position == path.Length)
                {
                    if (pathLength < bestOrderLength)
                    {
                        BestOrder = path.ToArray();
                    }

                    return;
                }

                for (int i = 0; i < path.Length; i++)
                {
                    var index = Array.IndexOf(path, i, 0, position);
                    if (index != -1)
                        continue;
                    path[position] = i;
                    MakePermutations(checkpoints, path, position + 1);
                }
            }

            return;
		}

		private static int[] MakeTrivialPermutation(int size)
		{
			 BestOrder = new int[size];
			for (int i = 0; i < BestOrder.Length; i++)
				BestOrder[i] = i;
			return BestOrder;
		}
	}
}
