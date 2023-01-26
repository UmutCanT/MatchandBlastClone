using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    private Grid<Tile> grid;

    public Map(int width, int height, float cellSize, Vector3 gridOrigin)
    {
        grid = new Grid<Tile>(width, height, cellSize, gridOrigin, (Grid<Tile> g, int x, int y) => new Tile(g, x, y));
    }

    public void SetMapTile(Vector3 worldPos, TileTemplate tileTemplate)
    {
        Tile tile = grid.GetGridObject(worldPos);
        if (tile != null)
        {
            tile.SetTile(tileTemplate);
        }
    }

    public void SetMapTile(Vector3 worldPos, TileTypes tileTemplate)
    {
        Tile tile = grid.GetGridObject(worldPos);
        if (tile != null)
        {
            tile.SetTile(tileTemplate);
        }
    }
}
