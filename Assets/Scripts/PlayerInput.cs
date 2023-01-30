using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    Map map;

    private void Awake()
    {
        map = GameObject.FindGameObjectWithTag(Utils.MAP_TAG).GetComponent<Map>();
    }  
    public void Interact()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 position = Utils.GetMouseWorldPosition();
            map.TileGrid.GetXandY(position, out int x, out int y);
            map.PopUpTiles(map.SearchOneTile(x, y));
        }
    }
}
