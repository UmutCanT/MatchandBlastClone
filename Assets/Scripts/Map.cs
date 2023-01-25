using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    private Grid<Tile> grid;

    public Map()
    {
        grid = new Grid<Tile>(10,10,10f, Vector3.zero, (Grid<Tile> g, int x, int y) => new Tile(g, x, y));
    }
}
