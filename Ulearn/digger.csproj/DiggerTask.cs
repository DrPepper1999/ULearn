using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.ObjectModel;

namespace Digger
{
    public class Terrain : ICreature
    {
        public CreatureCommand Act(int x, int y)
        {
            return new CreatureCommand() { DeltaX = 0, DeltaY = 0};
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            if (conflictedObject.GetImageFileName() == "Digger.png")
                return true;

                return false;
        }

        public int GetDrawingPriority()
        {
            return 5;
        }

        public string GetImageFileName()
        {
            return "Terrain.png";
        }
    }

    public class Player : ICreature
    {
        public CreatureCommand Act(int x, int y)
        {
            if (Game.KeyPressed == Keys.Right && x + 1 < Game.MapWidth && !IsEntity(x+1, y, "Sack.png"))
                return new CreatureCommand() { DeltaX = 1, DeltaY = 0 };
            if (Game.KeyPressed == Keys.Left && x - 1 > -1 && !IsEntity(x - 1, y, "Sack.png"))
                return new CreatureCommand() { DeltaX = -1, DeltaY = 0 };
            if (Game.KeyPressed == Keys.Up && y - 1 > -1 && !IsEntity(x, y-1, "Sack.png"))
                return new CreatureCommand() { DeltaX = 0, DeltaY = -1 };
            if (Game.KeyPressed == Keys.Down && y + 1 < Game.MapHeight && !IsEntity(x, y+1, "Sack.png"))
                return new CreatureCommand() { DeltaX = 0, DeltaY = 1 };
            return new CreatureCommand() { DeltaX = 0, DeltaY = 0 };
        }

        private static bool IsEntity(int x, int y, string entity)
        {
            return (Game.Map[x, y]?.GetImageFileName() ?? "") == entity;
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            if (conflictedObject.GetImageFileName() == "Sack.png" ||
                conflictedObject.GetImageFileName() == "Monster.png")
                return true;

            return false;
        }

        public int GetDrawingPriority()
        {
            return 4;
        }

        public string GetImageFileName()
        {
            return "Digger.png";
        }
    }

    public class Sack : ICreature
    {

        private int countCellsFlown = 0;
        public CreatureCommand Act(int x, int y)
        {

            if (y + 1 < Game.MapHeight)
            {
                var entityBelow = Game.Map[x, y + 1];
                if (IsEntity(x, y + 1, "") || ((IsEntity(x, y + 1, "Digger.png") ||
                    IsEntity(x, y + 1, "Monster.png")) && countCellsFlown > 0))
                {
                    countCellsFlown++;
                    return new CreatureCommand() { DeltaY = 1 };
                }
            }
            if (countCellsFlown > 1)
                return new CreatureCommand() { TransformTo = new Gold() };

            countCellsFlown = 0;
            return new CreatureCommand();
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            return false;
        }

        public int GetDrawingPriority()
        {
            return 3;
        }

        public string GetImageFileName()
        {
            return "Sack.png";
        }

        private static bool IsEntity(int x, int y, string entity)
        {
            return (Game.Map[x, y]?.GetImageFileName() ?? "") == entity;
        }
    }

    public class Gold : ICreature
    {
        public CreatureCommand Act(int x, int y)
        {
                return new CreatureCommand();
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            if (conflictedObject.GetImageFileName() == "Digger.png")
            {
                Game.Scores += 10;
                return true;
            }

            if (conflictedObject.GetImageFileName() == "Monster.png")
                return true;

                return false;
        }

        public int GetDrawingPriority()
        {
            return 2;
        }

        public string GetImageFileName()
        {
            return "Gold.png";
        }

        private static bool IsEntity(int x, int y, string entity)
        {
            return (Game.Map[x, y]?.GetImageFileName() ?? string.Empty) == entity;
        }
    }

    public class Monster : ICreature
    {
        public CreatureCommand Act(int x, int y)
        {
            Point pointDigger;
            if (IsEntityAllMap(Game.Map, "Digger.png", out pointDigger))
            {
                var patch = Route.FindPath(Game.Map, new Point(x, y), pointDigger);
                if (patch != null)
                    return new CreatureCommand() { DeltaX = patch[1].X - x, DeltaY = patch[1].Y - y };
                else
                    return new CreatureCommand();
            }
            else
            {
                return new CreatureCommand();
            }
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            if (conflictedObject.GetImageFileName() == "Sack.png" ||
                conflictedObject.GetImageFileName() == "Monster.png")
                return true;

            return false;        
        }

        public int GetDrawingPriority()
        {
            return 3;
        }

        public string GetImageFileName()
        {
            return "Monster.png";
        }

        private bool IsEntityAllMap(ICreature[,] map, string entity, out Point point)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i, j]?.GetImageFileName() == entity)
                    {
                        point = new Point(i, j);
                        return true;
                    }
                }
            }
            point = new Point();
            return false;
        }

        private static bool IsEntity(int x, int y, string entity)
        {
            return (Game.Map[x, y]?.GetImageFileName() ?? string.Empty) == entity;
        }
    }

    public class PathNode
    {
        public Point Position { get; set; }
        public int PathLengthFromStart { get; set; }
        public PathNode CameFrom { get; set; }
        public int HeuristicEstimatePathLength { get; set; }
        public int EstimateFullPathLength
        {
            get
            {
                return this.PathLengthFromStart + this.HeuristicEstimatePathLength;
            }
        }
    }

    public class Route
    {
        public static List<Point> FindPath(ICreature[,] map, Point start, Point goal)
        {
            var closedSet = new Collection<PathNode>();
            var openSet = new Collection<PathNode>();
            PathNode startNode = new PathNode()
            {
                Position = start,
                CameFrom = null,
                PathLengthFromStart = 0,
                HeuristicEstimatePathLength = GetHeuristicPathLength(start, goal)
            };
            openSet.Add(startNode);
            while (openSet.Count > 0)
            {
                var currentNode = openSet.OrderBy(node =>
                  node.EstimateFullPathLength).First();

                if (currentNode.Position == goal)
                    return GetPathForNode(currentNode);

                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                foreach (var neighbourNode in GetNeighbours(currentNode, goal, map))
                {
                    if (closedSet.Count(node => node.Position == neighbourNode.Position) > 0)
                        continue;
                    var openNode = openSet.FirstOrDefault(node =>
                      node.Position == neighbourNode.Position);
                    if (openNode == null)
                        openSet.Add(neighbourNode);
                    else
                      if (openNode.PathLengthFromStart > neighbourNode.PathLengthFromStart)
                    {
                        openNode.CameFrom = currentNode;
                        openNode.PathLengthFromStart = neighbourNode.PathLengthFromStart;
                    }
                }
            }

            return null;
        }

        private static int GetDistanceBetweenNeighbours()
        {
            return 1;
        }

        private static int GetHeuristicPathLength(Point from, Point to)
        {
            return Math.Abs(from.X - to.X) + Math.Abs(from.Y - to.Y);
        }

        private static Collection<PathNode> GetNeighbours(PathNode pathNode,
  Point goal, ICreature[,] map)
        {
            var result = new Collection<PathNode>();

            Point[] neighbourPoints = new Point[4];
            neighbourPoints[0] = new Point(pathNode.Position.X + 1, pathNode.Position.Y);
            neighbourPoints[1] = new Point(pathNode.Position.X - 1, pathNode.Position.Y);
            neighbourPoints[2] = new Point(pathNode.Position.X, pathNode.Position.Y + 1);
            neighbourPoints[3] = new Point(pathNode.Position.X, pathNode.Position.Y - 1);

            foreach (var point in neighbourPoints)
            {
                if (point.X < 0 || point.X >= map.GetLength(0))
                    continue;
                if (point.Y < 0 || point.Y >= map.GetLength(1))
                    continue;
                if ((map[point.X, point.Y]?.GetImageFileName() == "Terrain.png") ||
                    (map[point.X, point.Y]?.GetImageFileName() == "Sack.png") ||
                    (map[point.X, point.Y]?.GetImageFileName() == "Monster.png"))
                    continue;

                var neighbourNode = new PathNode()
                {
                    Position = point,
                    CameFrom = pathNode,
                    PathLengthFromStart = pathNode.PathLengthFromStart +
                    GetDistanceBetweenNeighbours(),
                    HeuristicEstimatePathLength = GetHeuristicPathLength(point, goal)
                };
                result.Add(neighbourNode);
            }
            return result;
        }

        private static List<Point> GetPathForNode(PathNode pathNode)
        {
            var result = new List<Point>();
            var currentNode = pathNode;
            while (currentNode != null)
            {
                result.Add(currentNode.Position);
                currentNode = currentNode.CameFrom;
            }
            result.Reverse();
            return result;
        }
    }
}
