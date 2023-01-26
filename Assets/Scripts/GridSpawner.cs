using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.WSA;

public class GridSpawner : MonoBehaviour
{
    private Map map;
    [SerializeField] TileTemplate blue;
    [SerializeField] TileTemplate green;
    [SerializeField] TileTemplate pink;
    [SerializeField] TileTemplate purple;
    [SerializeField] TileTemplate yellow;
    [SerializeField] TileTemplate red;

    // Start is called before the first frame update
    void Start()
    {      
        map = new Map(20,10,10f,Vector3.zero);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPos = Utils.GetMouseWorldPosition();
            map.SetMapTile(worldPos, TileTypes.Red);            
        }
    }
}
