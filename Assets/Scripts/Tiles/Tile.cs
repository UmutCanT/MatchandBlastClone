using System;
using UnityEngine;

public class Tile
{
    public event EventHandler OnPopped;   

    private TileTemplate tileTemp;
    private int x; 
    private int y;
    private TileStates tileState;

    public Tile(TileTemplate tileTemp, int x, int y)
    {
        this.tileTemp = tileTemp;
        this.x = x;
        this.y = y;
        tileState= TileStates.None;
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
        OnPopped?.Invoke(this, EventArgs.Empty);
    }
    public override string ToString()
    {
        return $"({x},{y})";
    }
}
