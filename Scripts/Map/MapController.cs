using System.Collections.Generic;
using BitRoguelike.Scripts.Map.Objects;
using BitRoguelike.Scripts.Util;
using Godot;

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

    public override void _Ready()
    {
        wallTileMap = GetNode<TileMap>(wallTileMapPath);
        floorTileMap = GetNode<TileMap>(floorTileMapPath);
        doodadTileMap = GetNode<TileMap>(doodadTileMapPath);
        doorContainer = GetNode(doorContainerPath);
        
        MapDoors();
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

    public bool HasClosedDoor(int x, int y)
    {
        Vector2I key = new Vector2I(x, y);
        if (doorMapping.ContainsKey(key))
        {
            return doorMapping[key].GetDoorState() != DoorState.OPENED;
        }

        return false;
    }
}