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

public class Tile
{
    public event EventHandler OnPopped;   

    private TileTemplate tileTemp;
    private int x; 
    private int y;
    private bool isPopped;
    private TileStates tileState;


    public Tile(TileTemplate tileTemp, int x, int y)
    {
        this.tileTemp = tileTemp;
        this.x = x;
        this.y = y;
        tileState= TileStates.None;
        isPopped = false;
    }

    public TileTemplate TileTemp { get => tileTemp; set => tileTemp = value; }
    public TileStates TileState { get => tileState; set => tileState = value; }

    public Vector3 GetWorldPosition()
    {
        return new Vector3 (x, y);
    }

    public void SetXandY(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public void PopTile()
    {
        isPopped= true;
        OnPopped?.Invoke(this, EventArgs.Empty);
    }
    public override string ToString()
    {
        return $"({x},{y})";
    }
}
