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
        map.CreateMap(10, 10, 1f, Vector3.zero);
    }

    private void Update()
    {       
        if(Input.GetMouseButtonDown(0)) 
        {
            Vector3 mousePosition = Utils.GetMouseWorldPosition();
            //visualsManager.PopUpTiles(map.TileGrid.GetGridObject(mousePosition));
        }
    }   
}
