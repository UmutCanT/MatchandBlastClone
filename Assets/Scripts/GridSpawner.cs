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
            map.TileGrid.GetXandY(mousePosition, out int x, out int y);
            map.PopUpTiles(map.SearchOneTile(x, y));        
        }
    }
    
    IEnumerator Waiting(int x, int y)
    {
        map.PopUpTiles(map.SearchOneTile(x, y));
        yield return new WaitForSeconds(2f);
        map.FillEmptyTilePositions();
        yield return new WaitForSeconds(2f);
        map.SpawnNewTiles();
        yield return new WaitForSeconds(2f);
        visualsManager.UpdateVisuals();
    }
}
