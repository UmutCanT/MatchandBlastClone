using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.WSA;

public class GridSpawner : MonoBehaviour
{
    [SerializeField] GridVisualsManager visualsManager;
    Vector3 spawnPos= new Vector3(-9f, -14f);
    private Map map;  

    // Start is called before the first frame update
    void Start()
    {
        map = new Map(7,10,2.5f,spawnPos);
        visualsManager.Setup(map.TileGrid);
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            map.TileGrid.CycleArray();
        }

        if(Input.GetMouseButtonDown(0)) 
        {
            Vector3 mousePosition = Utils.GetMouseWorldPosition();
            visualsManager.PopUpTiles(map.TileGrid.GetGridObject(mousePosition));
        }
    }   
}
