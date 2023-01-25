using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    
    int height;
    int width;
    float cellSize;
    int[,] gridArray;
    TextMesh[,] debugTextArr;

    public Grid(int width, int height, float cellSize)
    {
        this.height = height;
        this.width = width;
        this.cellSize = cellSize;
        
        gridArray = new int[width, height];
        debugTextArr= new TextMesh[width, height];
        CycleArray();
    }

    void CycleArray()
    {
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                debugTextArr[x, y] = Utils.CreateWorldText(null, gridArray[x, y].ToString(), GetWorldPosition(x, y) + new Vector3(cellSize , cellSize) * .5f, 
                    30, Color.blue, TextAnchor.MiddleCenter, TextAlignment.Center, 5000);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.blue, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x +1, y), Color.blue, 100f);
                debugTextArr[x, y].text = $"({x}, {y})";
            }
        }
        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.blue, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.blue, 100f);

    }


    Vector3 GetWorldPosition(int x, int y) 
    {
        return new Vector3(x, y) * cellSize;
    }

    public void SetValue(int x, int y, int value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;
            debugTextArr[x, y].text = gridArray[x, y].ToString();
        }
    }

    public void SetValue(Vector3 worldPos, int value)
    {
        int x, y;
        GetXandY(worldPos, out x, out y);
        SetValue(x, y, value);
    }

    void GetXandY(Vector3 worldPos, out int x, out int y)
    {
        x = Mathf.FloorToInt(worldPos.x/ cellSize);
        y = Mathf.FloorToInt(worldPos.y/ cellSize);
    }

    public int GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        else
            return 0;
    }

    public int GetValue(Vector3 worldPos)
    {
        int x, y;
        GetXandY(worldPos, out x, out y);
        return GetValue(x, y);
    }
}
