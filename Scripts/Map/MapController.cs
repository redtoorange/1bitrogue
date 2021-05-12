using System.Collections.Generic;
using BitRoguelike.Scripts.Map.Objects;
using BitRoguelike.Scripts.Systems.PathFinding;
using BitRoguelike.Scripts.Util;
using Godot;
using GraphNode = Godot.GraphNode;

public class MapController : Node2D
{
    [Export] private NodePath wallTileMapPath;
    [Export] private NodePath floorTileMapPath;
    [Export] private NodePath doodadTileMapPath;
    [Export] private NodePath doorContainerPath;
    
    private TileMap wallTileMap;
    private TileMap floorTileMap;
    private TileMap doodadTileMap;
    private Node doorContainer;

    private Dictionary<Vector2I, Door> doorMapping;
    
    private TileMapGraph tileMapGraph;

    public override void _Ready()
    {
        wallTileMap = GetNode<TileMap>(wallTileMapPath);
        floorTileMap = GetNode<TileMap>(floorTileMapPath);
        doodadTileMap = GetNode<TileMap>(doodadTileMapPath);
        doorContainer = GetNode(doorContainerPath);
        
        MapDoors();

        tileMapGraph = new TileMapGraph(this);
    }

    private void MapDoors()
    {
        doorMapping = new Dictionary<Vector2I, Door>();

        for (int i = 0; i < doorContainer.GetChildCount(); i++)
        {
            Door door = doorContainer.GetChild<Door>(i);
            Vector2 cellPos = wallTileMap.WorldToMap(door.GlobalPosition);

            doorMapping[new Vector2I(cellPos)] = door;
        }
    }

    public TileMap GetWallTileMap()
    {
        return wallTileMap;
    }

    public TileMap GetFloorTileMap()
    {
        return floorTileMap;
    }

    public TileMap GetDoodadTileMap()
    {
        return doodadTileMap;
    }

    public bool HasClosedDoor(int cellPosX, int cellPosY)
    {
        Vector2I key = new Vector2I(cellPosX, cellPosY);
        if (doorMapping.ContainsKey(key))
        {
            return doorMapping[key].GetDoorState() != DoorState.OPENED;
        }

        return false;
    }

    public List<Vector2> GeneratePath(Vector2 start, Vector2 end)
    {
        uint nodeConversion = OS.GetTicksMsec();
        TileMapGraphNode startNode = tileMapGraph.GetGraphNodeFromWorldPos(start);
        TileMapGraphNode endNode = tileMapGraph.GetGraphNodeFromWorldPos(end);
        GD.Print("Node Conversion: " + (OS.GetTicksMsec() - nodeConversion) + "ms");
        
        uint astarSearch = OS.GetTicksMsec();
        List<TileMapGraphNode> graphNodePath = AStarSearch.GeneratePath(tileMapGraph, startNode, endNode);
        GD.Print("AStarSearch.GeneratePath: " + (OS.GetTicksMsec() - astarSearch) + "ms");
        
        List<Vector2> path = new List<Vector2>();
        if (graphNodePath != null)
        {
            for (int i = 0; i < graphNodePath.Count; i++)
            {
                path.Add(graphNodePath[i].GetWorldPosition());
            }
        }
        return path;
    }
}