using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map
{
    private Grid<Tile> tileGrid;
    public Map(int width, int height, float cellSize, Vector3 gridOrigin)
    {
        tileGrid = new Grid<Tile>(width, height, cellSize, gridOrigin, (Grid<Tile> g, int x, int y) => new Tile(g, x, y));
    }
    
    public Grid<Tile> TileGrid { get => tileGrid; }
}
