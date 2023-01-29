using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Map: MonoBehaviour 
{
    [SerializeField] TileTemplate[] tileTemplates;

    public event EventHandler OnGemGridPositionDestroyed;
    public event EventHandler<OnNewGemGridSpawnedEventArgs> OnNewGemGridSpawned;
    public event EventHandler<OnLevelSetEventArgs> OnLevelSet;

    public class OnNewGemGridSpawnedEventArgs : EventArgs
    {
        public Tile tile;
        public TilePosition tilePosition;
    }

    public class OnLevelSetEventArgs : EventArgs
    {
        public Grid<TilePosition> tilePositionGrid;
    }

    Grid<TilePosition> tilePositionGrid;
    private int gridWidth;
    private int gridHeight;
    private int stateC = 5;
    private int stateB = 3;
    private int stateA = 1;
    int sameColorCount = 0;
    List<TilePosition> sameColorsList;
    List<TilePosition> noSameNeighbor;

    public Grid<TilePosition> TileGrid { get => tilePositionGrid; }

    public void CreateMap(int width, int height, float cellSize, Vector3 gridOrigin)
    {
        gridWidth = width;
        gridHeight = height;
        tilePositionGrid = new Grid<TilePosition>(gridWidth, gridHeight, cellSize, gridOrigin, (Grid<TilePosition> g, int x, int y) => new TilePosition(g, x, y));
        sameColorsList = new List<TilePosition>();
        noSameNeighbor = new List<TilePosition>();

        //Initialize TileGrid
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                TileTemplate tileTemplate = tileTemplates[Random.Range(0, tileTemplates.Length)];
                Tile tile = new Tile(tileTemplate, x, y);
                tilePositionGrid.GetGridObject(x, y).SetTile(tile);
            }
        }
        CheckAllTiles();
        OnLevelSet?.Invoke(this, new OnLevelSetEventArgs { tilePositionGrid = tilePositionGrid });
    }  
    
    public void SpawnNewTiles()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                TilePosition tilePosition = tilePositionGrid.GetGridObject(x, y);
                if (tilePosition.IsEmpty())
                {
                    TileTemplate tileTemplate = tileTemplates[Random.Range(0, tileTemplates.Length)];
                    Tile tile = new Tile(tileTemplate, x, y);

                    tilePosition.SetTile(tile);

                    OnNewGemGridSpawned?.Invoke(tile, new OnNewGemGridSpawnedEventArgs
                    {
                        tile = tile,
                        tilePosition = tilePosition
                    });
                }
            }
        }
    }

    public void FillEmptyTilePositions()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                TilePosition tilePosition = tilePositionGrid.GetGridObject(x, y);

                if (!tilePosition.IsEmpty())
                {
                    for (int i = y -1; i >= 0; i--)
                    {
                        TilePosition nextTilePosition = tilePositionGrid.GetGridObject(x, i);
                        if (nextTilePosition.IsEmpty())
                        {
                            tilePosition.Tile.SetXandY(x, i);
                            nextTilePosition.SetTile(tilePosition.Tile);
                            tilePosition.PopTile();

                            tilePosition = nextTilePosition;
                        }
                        else
                        {
                            break;
                        }

                    }
                }
            }
        }
    }

    void CheckAllTiles()
    {
        for (int x = 0; x < gridWidth; x++)
        {            
            for (int y = 0; y < gridHeight; y++)
            {
                if (Searchable(x, y))
                {
                    SearchNeighbors(x, y);
                                        
                    if (sameColorCount > 0)
                    {                        
                        ListOfTilesStateDecider(sameColorCount, sameColorsList);
                    }                    
                    else
                    {
                        noSameNeighbor.Add(tilePositionGrid.GetGridObject(x, y));
                    }
                    sameColorsList.Clear();
                    sameColorCount = 0;
                }               
            }
        }
        ChangeListofTilesState(noSameNeighbor, TileStates.None);
        ClearSearch();
    }

    public void ClearSearch()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                tilePositionGrid.GetGridObject(x, y).Searched= false;
            }
        }

    }

    void ListOfTilesStateDecider(int sameColorCount, List<TilePosition> posList)
    {
        if (sameColorCount > stateC)
        {
            ChangeListofTilesState(posList, TileStates.C);
        }
        else if (sameColorCount > stateB && sameColorCount <= stateC)
        {
            ChangeListofTilesState(posList, TileStates.B);
        }
        else if (sameColorCount > stateA && sameColorCount <= stateB)
        {
            ChangeListofTilesState(posList, TileStates.A);
        }
    }

    void ChangeListofTilesState(List<TilePosition> posList, TileStates state)
    {
        foreach(TilePosition tilePos in posList)
        {
            tilePos.Tile.TileState= state;
        }
    }

    public void SearchNeighbors(int x, int y)
    {       
        TileTemplate temp = GetTemplate(x,y);

        //if (temp == null) return null;

        if (Searchable(x + 1, y))
        {
            TileTemplate nextTemplate = GetTemplate(x +1, y);
            Debug.Log(temp + " == " + nextTemplate);

            if (temp == nextTemplate)
            {
                sameColorCount++;
                sameColorsList.Add(tilePositionGrid.GetGridObject(x + 1, y));
                tilePositionGrid.GetGridObject(x + 1, y).Searched = true;
                SearchNeighbors(x + 1, y);
            }
        }

        if (Searchable(x - 1, y))
        {
            TileTemplate nextTemplate = GetTemplate(x - 1, y);
            Debug.Log(temp + " == "  + nextTemplate);

            if (temp == nextTemplate)
            {
                sameColorCount++;
                sameColorsList.Add(tilePositionGrid.GetGridObject(x - 1, y));
                tilePositionGrid.GetGridObject(x - 1, y).Searched = true;
                SearchNeighbors(x - 1, y);
            }
        }

        if (Searchable(x , y + 1))
        {
            TileTemplate nextTemplate = GetTemplate(x, y + 1);
            Debug.Log(temp + " == " + nextTemplate);
            if (temp == nextTemplate)
            {
                sameColorCount++;
                sameColorsList.Add(tilePositionGrid.GetGridObject(x, y + 1));
                tilePositionGrid.GetGridObject(x , y + 1).Searched = true;
                SearchNeighbors(x, y + 1);
            }
        }

        if (Searchable(x, y - 1))
        {
            TileTemplate nextTemplate = GetTemplate(x, y - 1);
            Debug.Log(temp + " == " + nextTemplate);
            if (temp == nextTemplate)
            {
                sameColorCount++;
                sameColorsList.Add(tilePositionGrid.GetGridObject(x, y - 1));
                tilePositionGrid.GetGridObject(x, y - 1).Searched = true;
                SearchNeighbors(x, y - 1);
            }
        }

        Debug.Log(sameColorCount);
    }

    bool Searchable(int x, int y)
    {
        if (x < 0 || x >= gridWidth || y < 0 || y >= gridHeight)
        {
            return false;
        }
        else
        {
            return !tilePositionGrid.GetGridObject(x, y).Searched;
        }
    }

    TileTemplate GetTemplate(int x, int y)
    {
        //if (!Searchable(x, y)) return null;
        
        TilePosition tilePosition = tilePositionGrid.GetGridObject(x ,y);

        if (tilePosition.Tile == null) return null;

        return tilePosition.Tile.TileTemp;
    }
}
