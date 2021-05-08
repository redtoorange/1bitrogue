using System.Collections.Generic;
using BitRoguelike.Scripts.Characters.Player;
using Godot;

public class VisionController : Node2D
{
    [Export] private NodePath visionTileMapPath;
    [Export] private int tileBuffer = 32;

    private TileMap visionTileMap;

    // Injected
    private Player player;
    private MapController mapController;

    public override void _Ready()
    {
        visionTileMap = GetNode<TileMap>(visionTileMapPath);
    }

    public void Init(Player player, MapController mapController)
    {
        this.player = player;
        this.mapController = mapController;

        FillMapHiddenTiles();
        InitialLosCalculation();
        
        Update();
    }


    private void FillMapHiddenTiles()
    {
        TileMap wallTileMap = mapController.GetWallTileMap();

        Rect2 usedRect = wallTileMap.GetUsedRect();
        Vector2 startPosition = usedRect.Position;
        Vector2 size = usedRect.Size;
        int startX = Mathf.FloorToInt(startPosition.x);
        int startY = Mathf.FloorToInt(startPosition.y);
        int endX = startX + Mathf.CeilToInt(size.x);
        int endY = startY + Mathf.CeilToInt(size.y);

        for (int x = startX - tileBuffer; x < endX + tileBuffer; x++)
        {
            for (int y = startY - tileBuffer; y < endY + tileBuffer; y++)
            {
                visionTileMap.SetCell(x, y, 1);
            }
        }
    }

    private void InitialLosCalculation()
    {
        PrecomputeValues();
        CalculateLos();
    }

    private int accuracy = 1;
    private float[] sinValues;
    private float[] cosValues;

    private void PrecomputeValues()
    {
        sinValues = new float[360 / accuracy];
        cosValues = new float[360 / accuracy];
        for (int i = 0; i < 360 / accuracy; i++)
        {
            float x = Mathf.Cos(Mathf.Deg2Rad(i * accuracy));
            cosValues[i] = x;

            float y = Mathf.Sin(Mathf.Deg2Rad(i * accuracy));
            sinValues[i] = y;
        }
    }

    private void CalculateLos()
    {
        for (int i = 0; i < 360 / accuracy; i++)
        {
            float x = cosValues[i];
            float y = sinValues[i];
            DoFov(x, y);
            visionTileMap.UpdateDirtyQuadrants();
        }
    }

    private struct LineTuple
    {
        public Vector2 start;
        public Vector2 end;

        public LineTuple(Vector2 start, Vector2 end)
        {
            this.start = start;
            this.end = end;
        }
    }

    private List<LineTuple> lines = new List<LineTuple>();

    private void DoFov(float x, float y)
    {
        Vector2 playerPosition = visionTileMap.WorldToMap(player.Position);
        float ox = playerPosition.x;
        float oy = playerPosition.y;

        for (int i = 0; i < 4; i++)
        {
            visionTileMap.SetCell((int) ox, (int) oy, -1);
            lines.Add(new LineTuple(
                 visionTileMap.MapToWorld(new Vector2((int) ox, (int) oy)),
                 visionTileMap.MapToWorld(new Vector2((int) ox + 1, (int) oy + 1))
            ));

            if (mapController.GetWallTileMap().GetCell((int) ox, (int) oy) != -1)
            {
                return;
            }

            ox += x;
            oy += y;
        }
    }

    public override void _Draw()
    {
        GD.Print("Drawing lines");
        for (int i = 0; i < lines.Count; i++)
        {
            GD.Print("Drawing line " + i);
            DrawLine(
                lines[i].start,
                lines[i].end,
                Colors.Red
            );
        }
    }
}