using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Tile
{
    private Grid<Tile> grid;
    private int x;
    private int y;
    private TileTemplate tileType;
    private TileTypes tileTypeEnum;

    public Tile(Grid<Tile> grid,  int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        tileTypeEnum = (TileTypes)Random.Range(0, Enum.GetValues(typeof(TileTypes)).Length);
    }

    public override string ToString()
    {
        return tileTypeEnum.ToString();
    }
}
