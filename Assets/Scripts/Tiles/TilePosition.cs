using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class TilePosition
{
    private Tile tile;
    private Grid<TilePosition> grid;
    private int x;
    private int y;
    private bool searched;

    public bool Searched { get => searched; set => searched = value; }
    public int X { get => x; }
    public int Y { get => y; }
    public Tile Tile { get => tile; }

    public TilePosition(Grid<TilePosition> grid,  int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        searched = false;
    }  
    public void SetXandY(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public void SetTile(Tile tile)
    {
        this.tile = tile;
        grid.TriggerGridObjectChanged(x, y);
    }
    
    public Vector3 GetWorldPosition()
    {
        return grid.GetWorldPosition(x, y);
    }

    public void PopTile()
    {
        tile?.PopTile();
        grid.TriggerGridObjectChanged(x, y);
    }

    public void ClearPosition()
    {
        tile = null;
        searched = false;
    }

    public override string ToString()
    {
        return tile?.ToString();
    }

    public bool HasTile() => tile != null;
    public bool IsEmpty() => tile == null;
}