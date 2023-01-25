using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpawner : MonoBehaviour
{
    Grid<Tile> tileGrid;

    // Start is called before the first frame update
    void Start()
    {      
        tileGrid = new Grid<Tile>(10,10, 10f,  new Vector3(-40,-40), (Grid<Tile> g, int x, int y) => new Tile(g, x, y));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 position = Utils.GetMouseWorldPosition();
            Tile  tile  = tileGrid.GetGridObject(position);
            if (tile != null)
            {
                tile.AddValue(5);
            }
        }
    }
}

public class Tile
{
    private Grid<Tile> grid;
    private int value;
    private int x;
    private int y;

    public Tile(Grid<Tile> grid,  int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }
    public void AddValue(int addValue)
    {
        value += addValue;
        grid.GridObjectChangeTrigger(x, y);
    }

    public override string ToString()
    {
        return value.ToString();
    }
}
