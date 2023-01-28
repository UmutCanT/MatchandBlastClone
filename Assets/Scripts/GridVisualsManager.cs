using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using Random = UnityEngine.Random;

public class GridVisualsManager : MonoBehaviour
{   
    public static GridVisualsManager Instance { get; private set; }

    [SerializeField] List<TileTemplate> tileTemplates;
    [SerializeField] Transform prefabVisualNode;
    List<Transform> visualNodeList;
    Transform[,] visualNodeArray;
    Grid<Tile> grid;
    int sameColorCount = 0;
    List<Tile> sameColors = new List<Tile>();

    private void Awake()
    {
        Instance = this;
        visualNodeList= new List<Transform>();
    }

    public void Setup(Grid<Tile> grid)
    {
        this.grid = grid;
        visualNodeArray = new Transform[grid.Width, grid.Height];
        Vector3 startPos = grid.GridOrigin;
        float distance = grid.CellSize;
        for (int x = 0; x < grid.Width; x++)
        {
            for (int y = 0; y < grid.Height; y++)
            {
                Vector3 gridPosition = startPos + distance * new Vector3(x +.5f, y +.5f);
                Transform visualNode = CreateVisualNode(gridPosition);
                visualNodeArray[x,y] = visualNode;
                visualNodeList.Add(visualNode);
            }
        }
        grid.OnGridObjectChanged += OnGridValueChanged;
        UpdateVisual(this.grid);
    }

    private void OnGridValueChanged(object sender, Grid<Tile>.OnGridObjectChangedEventArgs e)
    {
        UpdateVisual(grid);
    }

    public void UpdateVisual(Grid<Tile> grid)
    {
        for (int x = 0; x < grid.Width; x++)
        {
            for (int y = 0; y < grid.Height; y++)
            {
                Tile tile = grid.GetGridObject(x, y);
                if (!tile.Searched)
                {
                    TileTypes searchedType = tile.TileType;                
                    SearchNeighbors(searchedType, x, y, grid, sameColors);
                    if (sameColorCount > 0)
                    {
                        ChangeListOfTilesSprites(sameColorCount, sameColors);
                    }
                    sameColors.Clear();
                    sameColorCount = 0;
                }                
                Transform visualNode = visualNodeArray[x, y];
                visualNode.gameObject.SetActive(true);
                SetupVisualNode(visualNode, tile);
            }
        }
        ClearSearch(grid);
    }

    public void ClearSearch(Grid<Tile> grid)
    {
        for (int x = 0; x < grid.Width; x++)
        {
            for (int y = 0; y < grid.Height; y++)
            {
                grid.GetGridObject(x, y).Searched = false;
            }
        }

    }

    private void HideNodeVisuals()
    {
        foreach (Transform visualNodeTransform in visualNodeList)
        {
            visualNodeTransform.gameObject.SetActive(false);
        }
    }

    private Transform CreateVisualNode(Vector3 gridPosition)
    {
        Transform visualNodeTransform = Instantiate(prefabVisualNode, gridPosition, Quaternion.identity);
        return visualNodeTransform;
    }

    private void SetupVisualNode(Transform visualNode, Tile tile)
    {
        TileObject tileObject = visualNode.gameObject.GetComponent<TileObject>();
        switch (tile.TileType)
        {
            case TileTypes.Bonus:
                break;
            case TileTypes.None:
                tileObject.ChangeSprite();
                break;
            case TileTypes.Blue:
                tileObject.ChangeSprite(tileTemplates[0].GetSprite(tile.BonusType));
                break;
            case TileTypes.Green:
                tileObject.ChangeSprite(tileTemplates[1].GetSprite(tile.BonusType));
                break;
            case TileTypes.Pink:
                tileObject.ChangeSprite(tileTemplates[2].GetSprite(tile.BonusType));
                break;
            case TileTypes.Purple:
                tileObject.ChangeSprite(tileTemplates[3].GetSprite(tile.BonusType));
                break;
            case TileTypes.Red:
                tileObject.ChangeSprite(tileTemplates[4].GetSprite(tile.BonusType));
                break;
            case TileTypes.Yellow:
                tileObject.ChangeSprite(tileTemplates[5].GetSprite(tile.BonusType));
                break;
            default:
                break;
        }
    }

    bool Searchable(Tile tile, int x, int y)
    {
        if (x < 0 || x > grid.Width || y < 0 || y > grid.Height)
            return false;

        if(tile != null)
            return !tile.Searched;
        return false;
    }
    public void PopUpTiles(Tile tile)
    {
        List<Tile> tiles= new List<Tile>();
        TileTypes searchedType = tile.TileType;
        SearchNeighbors(searchedType, tile.X, tile.Y, grid, tiles);
        if (tiles.Count > 1)
        {
            foreach(Tile item in tiles) 
            {
                item.TileType = TileTypes.None;
            }
            StartCoroutine(FindNullTiles());
            StopCoroutine(FindNullTiles());
        }    
    }


    void SearchNeighbors(TileTypes tileType, int x, int y, Grid<Tile> grid, List<Tile> sameColors)
    {      
        if (Searchable(grid.GetGridObject(x + 1, y), x + 1 ,y))
        {
            if(tileType == grid.GetGridObject(x + 1, y).TileType)
            {
                sameColors.Add(grid.GetGridObject(x + 1, y));
                sameColorCount++;
                grid.GetGridObject(x + 1, y).Searched= true;
                SearchNeighbors(tileType, x + 1, y, grid, sameColors);
            }
        }

        if (Searchable(grid.GetGridObject(x - 1, y), x - 1, y))
        {
            if (tileType == grid.GetGridObject(x - 1, y).TileType)
            {
                sameColors.Add(grid.GetGridObject(x - 1, y));
                sameColorCount++;
                grid.GetGridObject(x - 1, y).Searched = true;
                SearchNeighbors(tileType, x - 1, y, grid, sameColors);
            }
        }

        if (Searchable(grid.GetGridObject(x, y + 1), x, y + 1))
        {
            if (tileType == grid.GetGridObject(x, y + 1).TileType)
            {
                sameColors.Add(grid.GetGridObject(x, y + 1));
                sameColorCount++;
                grid.GetGridObject(x, y + 1).Searched = true;
                SearchNeighbors(tileType, x, y + 1, grid, sameColors);
            }
        }

        if (Searchable(grid.GetGridObject(x, y - 1), x, y - 1))
        {
            if (tileType == grid.GetGridObject(x, y - 1).TileType)
            {
                sameColors.Add(grid.GetGridObject(x, y - 1));
                sameColorCount++;
                grid.GetGridObject(x, y - 1).Searched = true;
                SearchNeighbors(tileType, x, y - 1, grid, sameColors);
            }
        }        
    }

    void ChangeListOfTilesSprites(int sameColorCount, List<Tile> tiles)
    {
        if (sameColorCount > 5)
        {
            ListChanges(tiles, "C");           
        }
        else if (sameColorCount > 3 && sameColorCount <= 5)
        {
            ListChanges(tiles, "B");
        }
        else if (sameColorCount > 1 && sameColorCount <= 3)
        {
            ListChanges(tiles, "A");
        }
    }

    void ListChanges(List<Tile> tiles, string text)
    {
        foreach (var tile in tiles)
        {
            tile.BonusType = text;
        }

    }

    public IEnumerator FindNullTiles()
    {
        for (int x = 0; x < grid.Width; x++)
        {
            for (int y = 0; y < grid.Height; y++)
            {
                if(grid.GetGridObject(x, y).TileType == TileTypes.None)
                {
                    yield return StartCoroutine(ShiftTiles(x, y));
                    break;
                }
            }
        }
    }

    private IEnumerator ShiftTiles(int x, int yStart , float shifDelay = 0.15f)
    {
        int nullCount = 0;
        List<Tile> nullTiles = new List<Tile>();

        for (int y = yStart; y < grid.Height; y++)
        {  
            Tile tile = grid.GetGridObject(x, y);
            if (tile.TileType == TileTypes.None)
            { 
                nullCount++;
            }
            nullTiles.Add(tile);
        }

        for (int i = 0; i < nullCount; i++)
        { 
            yield return new WaitForSeconds(shifDelay);
            for (int k = 0; k < nullTiles.Count - 1; k++)
            {
                nullTiles[k].TileType = nullTiles[k + 1].TileType;
                nullTiles[k + 1].TileType = GetNewTiles();
            }
        }
        UpdateVisual(grid);
    }

    private TileTypes GetNewTiles()
    {
        return (TileTypes)Random.Range(0, Enum.GetValues(typeof(TileTypes)).Length - 3);
    }
}