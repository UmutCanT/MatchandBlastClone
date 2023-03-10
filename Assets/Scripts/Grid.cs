using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid<TGridObject>
{
    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;
    public class OnGridObjectChangedEventArgs : EventArgs
    {
        public int x;
        public int y;
    }

    int width;
    int height;
    float cellSize;
    Vector3 gridOrigin;
    TGridObject[,] gridArray;
    System.Random random = new System.Random();

    public int Height { get => height; }
    public int Width { get => width; }
    public float CellSize { get => cellSize; }
    public Vector3 GridOrigin { get => gridOrigin; }


    public Grid(int width, int height, float cellSize, Vector3 gridOrigin, Func<Grid<TGridObject>, int, int, TGridObject> createdGridObject)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.gridOrigin = gridOrigin;    
        
        gridArray = new TGridObject[width, height];      

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x, y] = createdGridObject(this, x, y);
            }
        }
        ShowDebug(false);       
    }

    public void ShowDebug(bool show)
    {
        if (show)
        {
            TextMesh[,] debugTextArray = new TextMesh[width, height];

            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridArray.GetLength(1); y++)
                {
                    debugTextArray[x, y] = Utils.CreateWorldText(gridArray[x, y]?.ToString(), null, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * .5f, 30, Color.black, TextAnchor.MiddleCenter);
                    debugTextArray[x, y].characterSize = .1f;
                    debugTextArray[x, y].fontSize = 30;
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                }
            }
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

            OnGridObjectChanged += (object sender, OnGridObjectChangedEventArgs eventArgs) => {
                debugTextArray[eventArgs.x, eventArgs.y].text = gridArray[eventArgs.x, eventArgs.y]?.ToString();
            };
        }
    }

    public void Shuffle()
    {
        Utils.Shuffle(random, gridArray);
    }

    public Vector3 GetWorldPosition(int x, int y) 
    {
        return new Vector3(x, y) * cellSize + gridOrigin;
    }

    public void SetGridObject(int x, int y, TGridObject value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;          
        }
    }

    public void SetGridObject(Vector3 worldPos, TGridObject value)
    {
        int x, y;
        GetXandY(worldPos, out x, out y);
        SetGridObject(x, y, value);
    }

    public void GetXandY(Vector3 worldPos, out int x, out int y)
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

    public void TriggerGridObjectChanged(int x, int y)
    {
        OnGridObjectChanged?.Invoke(this, new OnGridObjectChangedEventArgs { x = x, y = y });
    }
}
