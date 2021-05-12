using System.Collections.Generic;
using Godot;
using Priority_Queue;

namespace BitRoguelike.Scripts.Systems.PathFinding
{
    class TileGraphNodeQueueNode : FastPriorityQueueNode
    {
        public TileMapGraphNode node;

        public TileGraphNodeQueueNode(TileMapGraphNode node)
        {
            this.node = node;
        }
    }
    
    public class AStarSearch
    {
        static public float Heuristic(TileMapGraphNode a, TileMapGraphNode b)
        {
            Vector2 aPos = a.GetTileMapPosition();
            Vector2 bPos = b.GetTileMapPosition();
            return Mathf.Abs(aPos.x - bPos.x) + Mathf.Abs(aPos.y - bPos.y);
        }

        /// <summary>
        /// Generate a path from Start to Goal using the AStar pathfinding algorithm.
        /// </summary>
        /// <param name="mapGraph">TileMapGraph to search</param>
        /// <param name="start">Starting TileMapGraphNode</param>
        /// <param name="goal">Ending TileMapGraphNode</param>
        /// <returns>Null if no path, or a List of TileMapGraphNodes, that form a path from the start(inclusive) to the
        /// goal (inclusive)</returns>
        public static List<TileMapGraphNode> GeneratePath(
            TileMapGraph mapGraph,
            TileMapGraphNode start,
            TileMapGraphNode goal
        )
        {
            if (start == null || goal == null || goal.IsWall())
            {
                return null;
            }

            var cameFrom = new Dictionary<TileMapGraphNode, TileMapGraphNode>();
            var costSoFar = new Dictionary<TileMapGraphNode, float>();
            var frontier = new FastPriorityQueue<TileGraphNodeQueueNode>(250);
            frontier.Enqueue(new TileGraphNodeQueueNode(start), 0);
            cameFrom[start] = start;
            costSoFar[start] = 0;

            // Walk the graph from start to goal
            while (frontier.Count > 0)
            {
                TileMapGraphNode current = frontier.Dequeue().node;

                if (current.Equals(goal))
                {
                    break;
                }

                foreach (TileMapGraphNode next in current.GetNeighbors())
                {
                    if (next != null)
                    {
                        float newCost = costSoFar[current] + mapGraph.CalculateMovementCost(current, next);
                        if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
                        {
                            costSoFar[next] = newCost;
                            float priority = newCost + Heuristic(next, goal);
                            frontier.Enqueue(new TileGraphNodeQueueNode(next), priority);
                            cameFrom[next] = current;
                        }
                    }
                }
            }

            // Build the path from nodes
            List<TileMapGraphNode> path = null;
            if (cameFrom.ContainsKey(goal))
            {
                path = new List<TileMapGraphNode>();
                TileMapGraphNode current = goal;
                while (current != start)
                {
                    path.Add(current);
                    current = cameFrom[current];
                }

                path.Add(start);
                path.Reverse();
            }

            return path;
        }
    }
}