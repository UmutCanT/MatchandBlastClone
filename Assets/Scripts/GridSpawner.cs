using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpawner : MonoBehaviour
{
    Grid<int> intGrid;
    Grid<bool> boolGrid;

    // Start is called before the first frame update
    void Start()
    {
        intGrid = new Grid<int>(4,2, 10f,  new Vector3(70,0));
        boolGrid = new Grid<bool>(10,10, 10f,  new Vector3(-40,0));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            intGrid.SetValue(Utils.GetMouseWorldPosition(), 25);
        }

        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log(intGrid.GetValue(Utils.GetMouseWorldPosition()));
        }
    }
}
