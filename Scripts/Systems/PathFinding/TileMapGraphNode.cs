using System;
using System.Collections.Generic;
using BitRoguelike.Scripts.Util;
using Godot;

namespace BitRoguelike.Scripts.Systems.PathFinding
{
    /// <summary>
    /// Direction from the source tile the neighbor is.
    /// </summary>
    public enum NeighborDirection
    {
        NORTH = 0,
        NORTH_EAST = 1,
        NORTH_WEST = 2,
        SOUTH = 3,
        SOUTH_EAST = 4,
        SOUTH_WEST = 5,
        EAST = 6,
        WEST = 7,
    }

    /// <summary>
    /// Virtual tile that will be used for pathfinding.
    /// </summary>
    public class TileMapGraphNode
    {
        /// <summary>
        /// Actual position in the parent TileMap from the MapController
        /// </summary>
        private Vector2I tileMapPosition;
        
        /// <summary>
        /// Position in the RLTileMap's graph
        /// </summary>
        private Vector2I graphPosition;

        private Vector2 worldPosition;
        
        private TileMapGraphNode[] neighbors;

        private bool isWall = false;

        public TileMapGraphNode(Vector2I tileMapPosition, Vector2I graphPosition, Vector2 worldPosition, bool isWall)
        {
            this.tileMapPosition = tileMapPosition;
            this.graphPosition = graphPosition;
            this.worldPosition = worldPosition;
            this.isWall = isWall;
            
            neighbors = new TileMapGraphNode[Enum.GetValues(typeof(NeighborDirection)).Length];
        }

        public void SetNeighbor(TileMapGraphNode neighbor, NeighborDirection direction)
        {
            neighbors[(int) direction] = neighbor;
        }

        public TileMapGraphNode GetNeighbor(NeighborDirection direction)
        {
            return neighbors[(int) direction];
        }

        public Vector2I GetGraphPosition()
        {
            return graphPosition;
        }
        
        public Vector2 GetTileMapPosition()
        {
            return new Vector2(tileMapPosition.x, tileMapPosition.y);
        }

        public Vector2 GetWorldPosition()
        {
            return worldPosition;
        }

        public TileMapGraphNode[] GetNeighbors()
        {
            return neighbors;
        }

        public bool IsWall()
        {
            return isWall;
        }
    }
}