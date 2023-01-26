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


    private void Awake()
    {
        Instance = this;
        visualNodeList= new List<Transform>();
    }

    public void Setup(Grid<Tile> grid)
    {
        this.grid = grid;
        visualNodeArray = new Transform[grid.Width, grid.Height];

        for (int x = 0; x < grid.Width; x++)
        {
            for (int y = 0; y < grid.Height; y++)
            {
                Vector3 gridPosition = grid.CellSize * (new Vector3(x, y) + Vector3.one * .5f);
                Transform visualNode = CreateVisualNode(gridPosition);
                visualNodeArray[x,y] = visualNode;
                visualNodeList.Add(visualNode);
            }
        }

        //HideNodeVisuals();
        grid.OnGridObjectChanged += OnGridValueChanged;
    }

    private void OnGridValueChanged(object sender, Grid<Tile>.OnGridObjectChangedEventArgs e)
    {
        UpdateVisual(grid);
    }

    public void UpdateVisual(Grid<Tile> grid)
    {
        //HideNodeVisuals();
        for (int x = 0; x < grid.Width; x++)
        {
            for (int y = 0; y < grid.Height; y++)
            {
                Tile tileObject = grid.GetGridObject(x, y);
                Transform visualNode = visualNodeArray[x, y];
                visualNode.gameObject.SetActive(true);
                SetupVisualNode(visualNode, tileObject);
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

    private void SetupVisualNode(Transform visualNode, Tile tileObject)
    {
        TileObject obj = visualNode.gameObject.GetComponent<TileObject>();
        SpriteRenderer spriteRenderer= obj.GetComponent<SpriteRenderer>();
        switch (tileObject.TileTypeEnum)
        {
            case TileTypes.Bonus:
                break;
            case TileTypes.Grass:
                break;
            case TileTypes.Blue:
                Debug.Log("Blue");
                obj.ChangeSprite(tileTemplates[0].TileSprite);
                spriteRenderer.sprite = tileTemplates[0].TileSprite;
                break;
            case TileTypes.Green:
                Debug.Log("Green");
                obj.ChangeSprite(tileTemplates[1].TileSprite);
                break;
            case TileTypes.Pink:
                Debug.Log("Pink");
                obj.ChangeSprite(tileTemplates[2].TileSprite);
                break;
            case TileTypes.Purple:
                Debug.Log("Purple");
                obj.ChangeSprite(tileTemplates[3].TileSprite);
                break;
            case TileTypes.Red:
                Debug.Log("Red");
                obj.ChangeSprite(tileTemplates[4].TileSprite);
                break;
            case TileTypes.Yellow:
                Debug.Log("Yellow");
                obj.ChangeSprite(tileTemplates[5].TileSprite);
                break;
            default:
                break;
        }
    }
}