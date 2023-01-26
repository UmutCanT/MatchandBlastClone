using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.WSA;

public class GridSpawner : MonoBehaviour
{
    [SerializeField] GridVisualsManager visualsManager;
    private Map map;  

    // Start is called before the first frame update
    void Start()
    {      
        map = new Map(5,5,2.5f,Vector3.zero);
        visualsManager.Setup(map.TileGrid);
        visualsManager.UpdateVisual(map.TileGrid);
    }

    private void Update()
    {     
    }
}
