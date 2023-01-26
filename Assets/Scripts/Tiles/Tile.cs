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

    public TileTemplate TileType { get => tileType; set => tileType = value; }
    public TileTypes TileTypeEnum { get => tileTypeEnum; set => tileTypeEnum = value; }

    public Tile(Grid<Tile> grid,  int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        //tileTypeEnum = (TileTypes)Random.Range(0, Enum.GetValues(typeof(TileTypes)).Length);
    }

    public Sprite TileSprite() => tileType.TileSprite;
    
    public void SetTile(TileTypes tileTypeEnum)
    {
        this.tileTypeEnum = tileTypeEnum;
        grid.GridObjectChangeTrigger(x, y);
    }

    public void SetTile(TileTemplate tileType)
    {
        this.tileType = tileType;
        grid.GridObjectChangeTrigger(x, y);
    }

    public override string ToString()
    {
        return tileTypeEnum.ToString();
    }
}
