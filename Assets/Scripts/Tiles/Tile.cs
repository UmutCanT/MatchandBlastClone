using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Tile
{
    private Grid<Tile> grid;
    private int x;
    private int y;
    private TileTypes tileType;
    private bool searched;
    string bonusType;


    public TileTypes TileType { get => tileType; set => tileType = value; }
    public bool Searched { get => searched; set => searched = value; }
    public string BonusType { get => bonusType; set => bonusType = value; }
    public int X { get => x; }
    public int Y { get => y; }

    public Tile(Grid<Tile> grid,  int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y; 
        searched= false;
        tileType = (TileTypes)Random.Range(0, Enum.GetValues(typeof(TileTypes)).Length-3);
    }   
    
    public void SetTile(TileTypes tileType)
    {
        this.tileType = tileType;
        grid.GridObjectChangeTrigger(x, y);
    }

    public override string ToString()
    {
        return bonusType;
    }
}
