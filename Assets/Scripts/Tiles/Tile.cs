using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Tile
{
    private Grid<Tile> grid;
    private int x;
    private int y;
    private TileTypes tileType;

    public TileTypes TileTypeEnum { get => tileType; }

    public Tile(Grid<Tile> grid,  int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;       
        tileType = (TileTypes)Random.Range(0, Enum.GetValues(typeof(TileTypes)).Length-3);
    }
    
    public void SetTile(TileTypes tileType)
    {
        this.tileType = tileType;
        grid.GridObjectChangeTrigger(x, y);
    }

    public override string ToString()
    {
        return tileType.ToString();
    }
}
