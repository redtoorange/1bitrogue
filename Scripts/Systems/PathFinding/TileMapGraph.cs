using BitRoguelike.Scripts.Util;
using Godot;

namespace BitRoguelike.Scripts.Systems.PathFinding
{
    /// <summary>
    /// A Virtual tile map constructed from a Godot TileMap.
    /// </summary>
    public class TileMapGraph
    {
        private MapController mapController;
        private TileMap wallMap;
        private TileMap floorMap;
        private TileMap doodadMap;

        private Rect2 mapBounds;
        private TileMapGraphNode[,] tileGraph;

        public TileMapGraph(MapController mapController)
        {
            this.mapController = mapController;
            ConstructTileMap();
        }

        private void ConstructTileMap()
        {
            wallMap = mapController.GetWallTileMap();
            floorMap = mapController.GetFloorTileMap();
            doodadMap = mapController.GetDoodadTileMap();

            mapBounds = CalculateBounds(wallMap, floorMap, doodadMap);
            tileGraph = new TileMapGraphNode[(int) mapBounds.Size.x, (int) mapBounds.Size.y];

            PopulateTiles();
            BuildGraph();
        }

        private void PopulateTiles()
        {
            Vector2I pos = new Vector2I(mapBounds.Position);
            Vector2I size = new Vector2I(mapBounds.Size);

            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    Vector2I tileMapPos = new Vector2I(pos.x + x, pos.y + y);
                    tileGraph[x, y] = new TileMapGraphNode(
                        tileMapPos,
                        new Vector2I(x, y),
                        wallMap.MapToWorld(tileMapPos.AsVector2()),
                        wallMap.GetCell(tileMapPos.x, tileMapPos.y) != -1
                    );
                }
            }
        }

        private void BuildGraph()
        {
            Vector2I pos = new Vector2I(mapBounds.Position);
            Vector2I size = new Vector2I(mapBounds.Size);

            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    TileMapGraphNode currentTile = tileGraph[x, y];
                    if (currentTile != null && !currentTile.IsWall())
                    {
                        // NORTH
                        if (y > 0 && !tileGraph[x, y - 1].IsWall())
                        {
                            currentTile.SetNeighbor(tileGraph[x, y - 1], NeighborDirection.NORTH);
                        }
                        // NORTH_EAST
                        if (y > 0 && x < size.x - 1 && !tileGraph[x + 1, y - 1].IsWall())
                        {
                            currentTile.SetNeighbor(tileGraph[x + 1, y - 1], NeighborDirection.NORTH_EAST);
                        }

                        // NORTH_WEST
                        if (y > 0 && x > 0 && !tileGraph[x - 1, y - 1].IsWall())
                        {
                            currentTile.SetNeighbor(tileGraph[x - 1, y - 1], NeighborDirection.NORTH_WEST);
                        }

                        // SOUTH
                        if (y < size.y - 1 && !tileGraph[x, y + 1].IsWall())
                        {
                            currentTile.SetNeighbor(tileGraph[x, y + 1], NeighborDirection.SOUTH);
                        }
                        // SOUTH_EAST
                        if (y < size.y - 1 && x < size.x - 1 && !tileGraph[x + 1, y + 1].IsWall())
                        {
                            currentTile.SetNeighbor(tileGraph[x + 1, y + 1], NeighborDirection.SOUTH_EAST);
                        }

                        // SOUTH_WEST
                        if (y < size.y - 1 && x > 0 && !tileGraph[x - 1, y + 1].IsWall())
                        {
                            currentTile.SetNeighbor(tileGraph[x - 1, y + 1], NeighborDirection.SOUTH_WEST);
                        }

                        // EAST
                        if (x < size.x - 1 && !tileGraph[x + 1, y].IsWall())
                        {
                            currentTile.SetNeighbor(tileGraph[x + 1, y], NeighborDirection.EAST);
                        }

                        // WEST
                        if (x > 0 && !tileGraph[x - 1, y].IsWall())
                        {
                            currentTile.SetNeighbor(tileGraph[x - 1, y], NeighborDirection.WEST);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Calculate the maximum and minimum Start and End point of all TileMaps used.
        /// </summary>
        private Rect2 CalculateBounds(TileMap wallMap, TileMap floorMap, TileMap doodadMap)
        {
            Rect2 wallBounds = wallMap.GetUsedRect();
            Rect2 floorBounds = floorMap.GetUsedRect();
            Rect2 doodadBounds = doodadMap.GetUsedRect();

            Vector2 position = new Vector2();
            position.x = Mathf.Min(wallBounds.Position.x, Mathf.Min(floorBounds.Position.x, doodadBounds.Position.x));
            position.y = Mathf.Min(wallBounds.Position.y, Mathf.Min(floorBounds.Position.y, doodadBounds.Position.y));

            Vector2 size = new Vector2();
            size.x = Mathf.Max(wallBounds.Size.x, Mathf.Max(floorBounds.Size.x, doodadBounds.Size.x));
            size.y = Mathf.Max(wallBounds.Size.y, Mathf.Max(floorBounds.Size.y, doodadBounds.Size.y));

            return new Rect2(position, size);
        }

        /// <summary>
        /// Calculate the movement cost between two GraphNodes
        /// </summary>
        public float CalculateMovementCost(TileMapGraphNode from, TileMapGraphNode to)
        {
            if (to.IsWall())
            {
                return Mathf.Inf;
            }

            return from.GetTileMapPosition().DistanceSquaredTo(to.GetTileMapPosition());
        }

        /// <summary>
        /// Using the TileMap call location, find the matching GraphNode
        /// </summary>
        public TileMapGraphNode GetGraphNodeFromTileMap(Vector2 tileMapPos)
        {
            Vector2I pos = new Vector2I(mapBounds.Position);
            Vector2I tileMapPosI = new Vector2I((int) tileMapPos.x - pos.x, (int) tileMapPos.y - pos.y);

            if (tileMapPosI.x >= 0 && tileMapPosI.x < tileGraph.GetLength(0))
            {
                if (tileMapPosI.y >= 0 && tileMapPosI.y < tileGraph.GetLength(1))
                {
                    return tileGraph[tileMapPosI.x, tileMapPosI.y];
                }
            }

            return null;
        }
        
        public TileMapGraphNode GetGraphNodeFromWorldPos(Vector2 worldPos)
        {
            return GetGraphNodeFromTileMap(wallMap.WorldToMap(worldPos));
        }
    }
}