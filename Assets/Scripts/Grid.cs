using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid<TGridObject>
{
    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;
    public class OnGridObjectChangedEventArgs : EventArgs
    {
        public int x,y;
    }


    int height;
    int width;
    float cellSize;
    Vector3 gridOrigin;
    TGridObject[,] gridArray;
    TextMesh[,] debugTextArr;

    public int Height { get => height; }
    public int Width { get => width; }
    public float CellSize { get => cellSize; }
    public Vector3 GridOrigin { get => gridOrigin; }

    public Grid(int width, int height, float cellSize, Vector3 gridOrigin, Func<Grid<TGridObject>, int, int, TGridObject> createdGridObject)
    {
        this.height = height;
        this.width = width;
        this.cellSize = cellSize;
        this.gridOrigin = gridOrigin;
        
        gridArray = new TGridObject[width, height];
        debugTextArr= new TextMesh[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x, y] = createdGridObject(this, x, y);
            }
        }

        bool showTest = false;
        if (showTest)
            CycleArray();
    }

    public void CycleArray()
    {
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
               debugTextArr[x, y] = Utils.CreateWorldText(null, gridArray[x, y]?.ToString(), GetWorldPosition(x, y) + new Vector3(cellSize , cellSize) /2, 10, Color.black, TextAnchor.MiddleCenter, TextAlignment.Center, 1);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.blue, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x +1, y), Color.blue, 100f);               
            }
        }
        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.blue, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.blue, 100f);

        OnGridObjectChanged += (object sender, OnGridObjectChangedEventArgs eventArgs) =>
        {
            debugTextArr[eventArgs.x, eventArgs.y].text = gridArray[eventArgs.x, eventArgs.y].ToString();
        };

    }


    Vector3 GetWorldPosition(int x, int y) 
    {
        return new Vector3(x, y) * cellSize + gridOrigin;
    }

    public void SetGridObject(int x, int y, TGridObject value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;
            debugTextArr[x, y].text = gridArray[x, y].ToString();
        }
    }

    public void SetGridObject(Vector3 worldPos, TGridObject value)
    {
        int x, y;
        GetXandY(worldPos, out x, out y);
        SetGridObject(x, y, value);
    }

    void GetXandY(Vector3 worldPos, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPos - gridOrigin).x/ cellSize);
        y = Mathf.FloorToInt((worldPos - gridOrigin).y/ cellSize);
    }

    public TGridObject GetGridObject(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        else
            return default;
    }

    public TGridObject GetGridObject(Vector3 worldPos)
    {
        int x, y;
        GetXandY(worldPos, out x, out y);
        return GetGridObject(x, y);
    }

    public void GridObjectChangeTrigger(int x, int y)
    {
        if (OnGridObjectChanged != null)
            OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, y = y });
    }


    void ShowCoordinates(int x, int y)
    {
        debugTextArr[x, y].text = $"({x}, {y})";
    }
}
