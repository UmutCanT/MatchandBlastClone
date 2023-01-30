using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GridVisualsManager visualsManager;
    [SerializeField] Map map;

    // Start is called before the first frame update
    void Start()
    {
        map.CreateMap();
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
