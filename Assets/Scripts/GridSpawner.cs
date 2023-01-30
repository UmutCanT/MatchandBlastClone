using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpawner : MonoBehaviour
{
    [SerializeField] GridVisualsManager visualsManager;
    private Map map;

    private void Awake()
    {
        map = GetComponent<Map>();
    }

    // Start is called before the first frame update
    void Start()
    {
        map.CreateMap(3, 3, 1f, Vector3.zero);
    }

    private void Update()
    {       
        if(Input.GetMouseButtonDown(0)) 
        {
            Vector3 mousePosition = Utils.GetMouseWorldPosition();
            map.TileGrid.GetXandY(mousePosition, out int x, out int y);
            map.PopUpTiles(map.SearchOneTile(x, y));        
        }
    } 
}
