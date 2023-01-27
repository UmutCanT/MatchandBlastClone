using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            case TileTypes.Grass:
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
    void SearchTile(TileTypes tileType, int x, int y, Grid<Tile> grid, int colorCount, List<Tile> sameColors)
    {
        SearchNeighbors(tileType, x, y, grid, sameColors);      
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
            Debug.Log("C");
            ListChanges(tiles, "C");           
        }
        else if (sameColorCount > 3 && sameColorCount <= 5)
        {
            Debug.Log("B");
            ListChanges(tiles, "B");
        }
        else if (sameColorCount > 1 && sameColorCount <= 3)
        {
            Debug.Log("A");
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
}