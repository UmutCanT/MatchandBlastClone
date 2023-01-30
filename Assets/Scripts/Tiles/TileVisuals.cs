using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileVisuals : MonoBehaviour
{
    Map map;
    Grid<TilePosition> tileMap;
    CollectableSpawner collectableSpawner;

    public void Initialize(Map map, Grid<TilePosition> tileMap)
    {
        this.map = map;
        this.tileMap = tileMap;
        for (int x = 0; x < tileMap.Width; x++)
        {
            for (int y = 0; y < tileMap.Height; y++)
            {
                
            }
        }
    }
}
