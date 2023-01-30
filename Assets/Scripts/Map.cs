using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Map: MonoBehaviour 
{
    [SerializeField] LevelTemplete level;

    public event EventHandler OnGemGridPositionDestroyed;
    public event EventHandler<OnNewGemGridSpawnedEventArgs> OnNewGemGridSpawned;
    public event EventHandler<OnLevelSetEventArgs> OnLevelSet;
    public event Action OnPopUpFinished, OnFillTileFinish, OnTileSpawnFinish, OnCheckFinish;

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
    private int gridWidth = 10;
    private int gridHeight = 10;
    private int stateC = 10;
    private int stateB = 7;
    private int stateA = 5;
    int sameColorCount;
    List<TilePosition> sameColorsList;
    List<TilePosition> noSameNeighbor;
    bool hasPair = false;

    public Grid<TilePosition> TileGrid { get => tilePositionGrid; }

    void OnEnable()
    {
        OnPopUpFinished += FillEmptyTilePositions;
        OnPopUpFinished += ClearSearch;
        OnFillTileFinish += SpawnNewTiles;
        OnTileSpawnFinish += CheckAllTiles;
        OnCheckFinish += ChangePairValue;
    }

    void OnDisable()
    {
        OnPopUpFinished -= FillEmptyTilePositions;
        OnPopUpFinished -= ClearSearch;
        OnFillTileFinish -= SpawnNewTiles;
        OnTileSpawnFinish -= CheckAllTiles;
        OnCheckFinish -= ChangePairValue;
    }

    public void CreateMap()
    {
        gridWidth = level.Width;
        gridHeight = level.Height;
        stateA = level.StateA;
        stateB = level.StateB;
        stateC= level.StateC;
        tilePositionGrid = new Grid<TilePosition>(gridWidth, gridHeight, 1f, Vector3.zero, (Grid<TilePosition> g, int x, int y) => new TilePosition(g, x, y));
        sameColorsList = new List<TilePosition>();
        noSameNeighbor = new List<TilePosition>();

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                TileTemplate tileTemplate = level.TileTemplates[Random.Range(0, level.TileTemplates.Length)];
                Tile tile = new Tile(tileTemplate, x, y);
                tilePositionGrid.GetGridObject(x, y).SetTile(tile);
            }
        }
        CheckAllTiles();
        TileGrid.ShowDebug(false);
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
                    TileTemplate tileTemplate = level.TileTemplates[Random.Range(0, level.TileTemplates.Length)];
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
        OnTileSpawnFinish();
        OnCheckFinish();
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
                            tilePosition.ClearPosition();

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
        OnFillTileFinish();
    }
    void ChangePairValue()
    {
        hasPair = false;
    }

    public void CheckAllTiles()
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
                        hasPair = true;
                        ListOfTilesStateDecider(sameColorCount, sameColorsList);
                    }                    
                    else
                    {
                        noSameNeighbor.Add(TileGrid.GetGridObject(x, y));
                    }
                    sameColorsList.Clear();
                    sameColorCount = 0;
                }               
            }
        }
        ChangeListofTilesState(noSameNeighbor, TileStates.None);
        noSameNeighbor.Clear();
        ClearSearch();

        if(!hasPair) {
            
            ShuffleTiles();
        }
    }

    private void ShuffleTiles()
    {
        tilePositionGrid.Shuffle();
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {                
                tilePositionGrid.GetGridObject(x, y).Tile.SetXandY(x,y);
                tilePositionGrid.GetGridObject(x, y).SetXandY(x, y);
            }
        }
        CheckAllTiles();
        OnCheckFinish();
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

    public List<TilePosition> SearchOneTile(int x, int y)
    {
        SearchNeighbors(x, y);
        return sameColorsList;
    }

    public void PopUpTiles(List<TilePosition> tilePositions)
    {
        foreach (TilePosition tilePos in tilePositions)
        {
            if (tilePos.HasTile())
            {
                tilePos.PopTile();
                OnGemGridPositionDestroyed?.Invoke(tilePos, EventArgs.Empty);
                tilePos.ClearPosition();
            }
        }
        sameColorsList.Clear();
        sameColorCount = 0;
        OnPopUpFinished();
    }

    public void SearchNeighbors(int x, int y)
    {       
        TileTemplate temp = GetTemplate(x,y);

        if (Searchable(x + 1, y))
        {
            TileTemplate nextTemplate = GetTemplate(x +1, y);
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
            if (temp == nextTemplate)
            {
                sameColorCount++;
                sameColorsList.Add(tilePositionGrid.GetGridObject(x, y - 1));
                tilePositionGrid.GetGridObject(x, y - 1).Searched = true;
                SearchNeighbors(x, y - 1);
            }
        }             
    }

    bool Searchable(int x, int y)
    {
        if (OutofBounds(x, y))
        {
            return false;
        }
        else
        {
            return !tilePositionGrid.GetGridObject(x, y).Searched;
        }
    }

    bool OutofBounds(int x, int y)
    {
        if (x < 0 || x >= gridWidth || y < 0 || y >= gridHeight)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    TileTemplate GetTemplate(int x, int y)
    {
        if (OutofBounds(x, y)) return null;
        
        TilePosition tilePosition = tilePositionGrid.GetGridObject(x ,y);

        if (tilePosition.Tile == null) return null;

        return tilePosition.Tile.TileTemp;
    }  
}
