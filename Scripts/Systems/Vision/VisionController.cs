using System;
using System.Collections.Generic;
using BitRoguelike.Scripts.Characters.Player;
using BitRoguelike.Scripts.Util;
using Godot;

///  Octant data
///                 <br/>
///    \ 1 | 2 /    <br/>
///   8 \  |  / 3   <br/>
///   -----+-----   <br/>
///   7 /  |  \ 4   <br/>
///    / 6 | 5 \    <br/>
/// 1 = NNW, 2 =NNE, 3=ENE, 4=ESE, 5=SSE, 6=SSW, 7=WSW, 8 = WNW
public enum VisualOctant
{
    NORTH_NORTH_WEST, // 1
    NORTH_NORTH_EAST, // 2
    EAST_NORTH_EAST, // 3
    EAST_SOUTH_EAST, // 4
    SOUTH_SOUTH_EAST, // 5
    SOUTH_SOUTH_WEST, // 6
    WEST_SOUTH_WEST, // 7
    WEST_NORTH_WEST // 8
}

public class VisionController : Node2D
{
    [Export] private NodePath visionTileMapPath;
    [Export] private int tileBuffer = 32;
    [Export] private int visualRange = 6;

    private TileMap visionTileMap;

    // Injected
    private Player player;
    private MapController mapController;
    private TileMap wallTileMap;
    private List<Vector2I> currentlyVisibleCellCache;

    public override void _Ready()
    {
        currentlyVisibleCellCache = new List<Vector2I>();
        visionTileMap = GetNode<TileMap>(visionTileMapPath);
    }

    public void Init(Player player, MapController mapController)
    {
        this.player = player;
        this.mapController = mapController;
        wallTileMap = mapController.GetWallTileMap();

        ResetVision();
    }

    public void UpdateFov(Vector2 atPosition)
    {
        UpdateShroudedCells(GetVisibleCells(atPosition));
    }

    public void ResetVision()
    {
        FillMapHiddenTiles();
        UpdateFov(player.GlobalPosition);
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
                visionTileMap.SetCell(x, y, 0);
            }
        }
    }

    private void UpdateShroudedCells(List<Vector2I> newVisibleCells)
    {
        // Reveal cells and remove them from the old cache
        for (int i = 0; i < newVisibleCells.Count; i++)
        {
            Vector2I cell = newVisibleCells[i];
            visionTileMap.SetCell(cell.x, cell.y, -1);
        }

        // Shroud the cells remaining in the cache
        for (int i = 0; i < currentlyVisibleCellCache.Count; i++)
        {
            Vector2I cell = currentlyVisibleCellCache[i];

            if (!newVisibleCells.Contains(cell))
            {
                visionTileMap.SetCell(cell.x, cell.y, 1);
            }
        }

        // Flip caches
        currentlyVisibleCellCache = newVisibleCells;
    }


    private List<Vector2I> GetVisibleCells(Vector2 position)
    {
        Vector2I playerPosition = new Vector2I(visionTileMap.WorldToMap(position));
        List<Vector2I> visiblePoints = new List<Vector2I>();
        visiblePoints.Add(playerPosition);

        foreach (VisualOctant octant in Enum.GetValues(typeof(VisualOctant)))
        {
            ScanOctant(1, octant, 1.0, 0.0, playerPosition, visiblePoints);
        }

        return visiblePoints;
    }

    private void ScanOctant(int depth, VisualOctant octant, double startSlope, double endSlope,
        in Vector2I playerPosition, List<Vector2I> visiblePoints)
    {
        int visrange2 = visualRange * visualRange;
        int x = 0;
        int y = 0;


        switch (octant)
        {
            case VisualOctant.NORTH_NORTH_WEST:
                y = playerPosition.y - depth;
                if (y < GetStartY()) return;

                x = playerPosition.x - Convert.ToInt32((startSlope * Convert.ToDouble(depth)));
                if (x < GetStartX()) x = GetStartX();

                while (GetSlope(x, y, playerPosition.x, playerPosition.y, false) >= endSlope)
                {
                    // if (x,y) within visual range then
                    if (GetVisDistance(x, y, playerPosition.x, playerPosition.y) <= visrange2)
                    {
                        bool currentTileBlocked = !IsOpen(x, y);
                        bool previousTileBlocked = !IsOpen(x - 1, y);

                        // if (x,y) blocked and prior not blocked then
                        if (currentTileBlocked && !previousTileBlocked)
                        {
                            // Scan(depth + 1, startslope, new_endslope)
                            ScanOctant(depth + 1, octant, startSlope,
                                GetSlope(x - 0.5, y + 0.5, playerPosition.x, playerPosition.y, false),
                                playerPosition, visiblePoints
                            );
                        }
                        // if (x,y) not blocked and prior blocked then
                        else if (!currentTileBlocked && previousTileBlocked)
                        {
                            // new_startslope
                            startSlope = GetSlope(x - 0.5, y - 0.5, playerPosition.x, playerPosition.y, false);
                        }

                        visiblePoints.Add(new Vector2I(x, y));
                    }

                    x++;
                }

                x--;
                break;

            case VisualOctant.NORTH_NORTH_EAST:

                y = playerPosition.y - depth;
                if (y < GetStartY()) return;

                x = playerPosition.x + Convert.ToInt32((startSlope * Convert.ToDouble(depth)));
                if (x >= GetEndX()) x = GetEndX() - 1;

                while (GetSlope(x, y, playerPosition.x, playerPosition.y, false) <= endSlope)
                {
                    if (GetVisDistance(x, y, playerPosition.x, playerPosition.y) <= visrange2)
                    {
                        bool currentTileBlocked = !IsOpen(x, y);
                        bool previousTileBlocked = !IsOpen(x + 1, y);
                        if (currentTileBlocked && !previousTileBlocked)
                        {
                            ScanOctant(depth + 1, octant, startSlope,
                                GetSlope(x + 0.5, y + 0.5, playerPosition.x, playerPosition.y, false),
                                playerPosition, visiblePoints);
                        }
                        else if (!currentTileBlocked && previousTileBlocked)
                        {
                            startSlope = -GetSlope(x + 0.5, y - 0.5, playerPosition.x, playerPosition.y, false);
                        }

                        visiblePoints.Add(new Vector2I(x, y));
                    }

                    x--;
                }

                x++;
                break;

            case VisualOctant.EAST_NORTH_EAST:

                x = playerPosition.x + depth;
                if (x >= GetEndX()) return;

                y = playerPosition.y - Convert.ToInt32((startSlope * Convert.ToDouble(depth)));
                if (y < GetStartY()) y = GetStartY();

                while (GetSlope(x, y, playerPosition.x, playerPosition.y, true) <= endSlope)
                {
                    if (GetVisDistance(x, y, playerPosition.x, playerPosition.y) <= visrange2)
                    {
                        bool currentTileBlocked = !IsOpen(x, y);
                        bool previousTileBlocked = !IsOpen(x, y - 1);
                        if (currentTileBlocked && !previousTileBlocked)
                        {
                            ScanOctant(depth + 1, octant, startSlope,
                                GetSlope(x - 0.5, y - 0.5, playerPosition.x, playerPosition.y, true),
                                playerPosition, visiblePoints);
                        }
                        else if (!currentTileBlocked && previousTileBlocked)
                        {
                            startSlope = -GetSlope(x + 0.5, y - 0.5, playerPosition.x, playerPosition.y, true);
                        }

                        visiblePoints.Add(new Vector2I(x, y));
                    }

                    y++;
                }

                y--;
                break;

            case VisualOctant.EAST_SOUTH_EAST:

                x = playerPosition.x + depth;
                if (x >= GetEndX()) return;

                y = playerPosition.y + Convert.ToInt32((startSlope * Convert.ToDouble(depth)));
                if (y >= GetEndY()) y = GetEndY() - 1;

                while (GetSlope(x, y, playerPosition.x, playerPosition.y, true) >= endSlope)
                {
                    if (GetVisDistance(x, y, playerPosition.x, playerPosition.y) <= visrange2)
                    {
                        bool currentTileBlocked = !IsOpen(x, y);
                        bool previousTileBlocked = !IsOpen(x, y + 1);
                        if (currentTileBlocked && !previousTileBlocked)
                        {
                            ScanOctant(depth + 1, octant, startSlope,
                                GetSlope(x - 0.5, y + 0.5, playerPosition.x, playerPosition.y, true),
                                playerPosition, visiblePoints);
                        }
                        else if (!currentTileBlocked && previousTileBlocked)
                        {
                            startSlope = GetSlope(x + 0.5, y + 0.5, playerPosition.x, playerPosition.y, true);
                        }

                        visiblePoints.Add(new Vector2I(x, y));
                    }

                    y--;
                }

                y++;
                break;

            case VisualOctant.SOUTH_SOUTH_EAST:

                y = playerPosition.y + depth;
                if (y >= GetEndY()) return;

                x = playerPosition.x + Convert.ToInt32((startSlope * Convert.ToDouble(depth)));
                if (x >= GetEndX()) x = GetEndX() - 1;

                while (GetSlope(x, y, playerPosition.x, playerPosition.y, false) >= endSlope)
                {
                    if (GetVisDistance(x, y, playerPosition.x, playerPosition.y) <= visrange2)
                    {
                        bool currentTileBlocked = !IsOpen(x, y);
                        bool previousTileBlocked = !IsOpen(x + 1, y);
                        if (currentTileBlocked && !previousTileBlocked)
                        {
                            ScanOctant(depth + 1, octant, startSlope,
                                GetSlope(x + 0.5, y - 0.5, playerPosition.x, playerPosition.y, false),
                                playerPosition, visiblePoints);
                        }
                        else if (!currentTileBlocked && previousTileBlocked)
                        {
                            startSlope = GetSlope(x + 0.5, y + 0.5, playerPosition.x, playerPosition.y, false);
                        }

                        visiblePoints.Add(new Vector2I(x, y));
                    }

                    x--;
                }

                x++;
                break;

            case VisualOctant.SOUTH_SOUTH_WEST:

                y = playerPosition.y + depth;
                if (y >= GetEndY()) return;

                x = playerPosition.x - Convert.ToInt32((startSlope * Convert.ToDouble(depth)));
                if (x < GetStartX()) x = GetStartX();

                while (GetSlope(x, y, playerPosition.x, playerPosition.y, false) <= endSlope)
                {
                    if (GetVisDistance(x, y, playerPosition.x, playerPosition.y) <= visrange2)
                    {
                        bool currentTileBlocked = !IsOpen(x, y);
                        bool previousTileBlocked = !IsOpen(x - 1, y);
                        if (currentTileBlocked && !previousTileBlocked)
                        {
                            ScanOctant(depth + 1, octant, startSlope,
                                GetSlope(x - 0.5, y - 0.5, playerPosition.x, playerPosition.y, false),
                                playerPosition, visiblePoints);
                        }
                        else if (!currentTileBlocked && previousTileBlocked)
                        {
                            startSlope = -GetSlope(x - 0.5, y + 0.5, playerPosition.x, playerPosition.y, false);
                        }

                        visiblePoints.Add(new Vector2I(x, y));
                    }

                    x++;
                }

                x--;
                break;

            case VisualOctant.WEST_SOUTH_WEST:

                x = playerPosition.x - depth;
                if (x < GetStartX()) return;

                y = playerPosition.y + Convert.ToInt32((startSlope * Convert.ToDouble(depth)));
                if (y >= GetEndY()) y = GetEndY() - 1;

                while (GetSlope(x, y, playerPosition.x, playerPosition.y, true) <= endSlope)
                {
                    if (GetVisDistance(x, y, playerPosition.x, playerPosition.y) <= visrange2)
                    {
                        bool currentTileBlocked = !IsOpen(x, y);
                        bool previousTileBlocked = !IsOpen(x, y + 1);
                        if (currentTileBlocked && !previousTileBlocked)
                        {
                            ScanOctant(depth + 1, octant, startSlope,
                                GetSlope(x + 0.5, y + 0.5, playerPosition.x, playerPosition.y, true),
                                playerPosition, visiblePoints);
                        }
                        else if (!currentTileBlocked && previousTileBlocked)
                        {
                            startSlope = -GetSlope(x - 0.5, y + 0.5, playerPosition.x, playerPosition.y, true);
                        }

                        visiblePoints.Add(new Vector2I(x, y));
                    }

                    y--;
                }

                y++;
                break;

            case VisualOctant.WEST_NORTH_WEST:

                x = playerPosition.x - depth;
                if (x < GetStartX()) return;

                y = playerPosition.y - Convert.ToInt32((startSlope * Convert.ToDouble(depth)));
                if (y < GetStartY()) y = GetStartY();

                while (GetSlope(x, y, playerPosition.x, playerPosition.y, true) >= endSlope)
                {
                    if (GetVisDistance(x, y, playerPosition.x, playerPosition.y) <= visrange2)
                    {
                        bool currentTileBlocked = !IsOpen(x, y);
                        bool previousTileBlocked = !IsOpen(x, y - 1);
                        if (currentTileBlocked && !previousTileBlocked)
                        {
                            ScanOctant(depth + 1, octant, startSlope,
                                GetSlope(x + 0.5, y - 0.5, playerPosition.x, playerPosition.y, true),
                                playerPosition, visiblePoints);
                        }
                        else if (!currentTileBlocked && previousTileBlocked)
                        {
                            startSlope = GetSlope(x - 0.5, y - 0.5, playerPosition.x, playerPosition.y, true);
                        }

                        visiblePoints.Add(new Vector2I(x, y));
                    }

                    y++;
                }

                y--;
                break;
        }


        if (x < GetStartX())
            x = GetStartX();
        else if (x >= GetEndX())
            x = GetEndX() - 1;

        if (y < GetStartY())
            y = GetStartY();
        else if (y >= GetEndY())
            y = GetEndY() - 1;

        if (depth < visualRange && IsOpen(x, y))
            ScanOctant(depth + 1, octant, startSlope, endSlope, playerPosition, visiblePoints);
    }

    private double GetSlope(double pX1, double pY1, double pX2, double pY2, bool pInvert)
    {
        if (pInvert)
            return (pY1 - pY2) / (pX1 - pX2);
        else
            return (pX1 - pX2) / (pY1 - pY2);
    }

    private int GetVisDistance(int pX1, int pY1, int pX2, int pY2)
    {
        return ((pX1 - pX2) * (pX1 - pX2)) + ((pY1 - pY2) * (pY1 - pY2));
    }

    private bool IsOpen(int x, int y)
    {
        return wallTileMap.GetCell(x, y) == -1 && !mapController.HasClosedDoor(x, y);
    }

    private int GetStartX()
    {
        Rect2 used = visionTileMap.GetUsedRect();
        return (int) used.Position.x;
    }

    private int GetEndX()
    {
        Rect2 used = visionTileMap.GetUsedRect();
        return (int) used.End.x;
    }

    private int GetStartY()
    {
        Rect2 used = visionTileMap.GetUsedRect();
        return (int) used.Position.y;
    }

    private int GetEndY()
    {
        Rect2 used = visionTileMap.GetUsedRect();
        return (int) used.End.y;
    }
}