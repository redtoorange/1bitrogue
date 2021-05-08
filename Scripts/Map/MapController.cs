using Godot;

public class MapController : Node2D
{
    [Export] private NodePath wallTileMapPath;
    [Export] private NodePath floorTileMapPath;
    [Export] private NodePath doodadTileMapPath;

    private TileMap wallTileMap;
    private TileMap floorTileMap;
    private TileMap doodadTileMap;

    public override void _Ready()
    {
        wallTileMap = GetNode<TileMap>(wallTileMapPath);
        floorTileMap = GetNode<TileMap>(floorTileMapPath);
        doodadTileMap = GetNode<TileMap>(doodadTileMapPath);
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
}