using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpawner : MonoBehaviour
{
    Grid grid;

    // Start is called before the first frame update
    void Start()
    {
        grid = new Grid(4,2, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            grid.SetValue(Utils.GetMouseWorldPosition(), 25);
        }

        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log(grid.GetValue(Utils.GetMouseWorldPosition()));
        }
    }
}
