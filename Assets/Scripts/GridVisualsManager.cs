using System;
using System.Collections.Generic;
using UnityEngine;

public partial class GridVisualsManager : MonoBehaviour
{
    [SerializeField] List<TileTemplate> tileTemplates;
    [SerializeField] Transform prefabVisualNode;
    [SerializeField] Map map;
    CamManager camManager;
    Dictionary<Tile, TileVisual> tileGridDictionary;
    float spawnPosY;

    void Awake()
    {
        map.OnLevelSet += InitializeVisuals;
        camManager = GameObject.FindGameObjectWithTag(Utils.CAMERA_TAG).GetComponent<CamManager>();
    }

    void OnEnable()
    {
        map.OnCheckFinish += UpdateVisuals;
    }

    void Update()
    {
        UpdateVisualsPositions();
    }

    void OnDisable()
    {
        map.OnCheckFinish -= UpdateVisuals;
    }

    void InitializeVisuals(object sender, Map.OnLevelSetEventArgs e)
    {
        Initialize(sender as Map, e.tilePositionGrid);
    }

    public void Initialize(Map map, Grid<TilePosition> grid)
    {
        this.map = map;
        spawnPosY = grid.Height + .5f;

        camManager.ChangeCamPosition(grid.Width, grid.Height);
        map.OnGemGridPositionDestroyed += RemoveTileFromDictionary;
        map.OnNewGemGridSpawned += InstantiateVisual;

        tileGridDictionary= new Dictionary<Tile, TileVisual>();

        for (int x = 0; x < grid.Width; x++)
        {
            for (int y = 0; y < grid.Height; y++)
            {
                TilePosition tilePosition = grid.GetGridObject(x, y);
                Tile tile = tilePosition.Tile;

                Vector3 position = grid.GetWorldPosition(x, y);
                position = new Vector3(position.x, spawnPosY);

                Transform tileGridVisualTransform = Instantiate(prefabVisualNode, position, Quaternion.identity);
                tileGridVisualTransform.GetComponentInChildren<SpriteRenderer>().sprite = tile.TileTemp.GetSprite(tile.TileState);

                TileVisual tileVisual = new TileVisual(tileGridVisualTransform, tile);

                tileGridDictionary[tile] = tileVisual;               
            }
        }   
    }

    void InstantiateVisual(object sender, Map.OnNewGemGridSpawnedEventArgs e)
    {
        Vector3 position = e.tilePosition.GetWorldPosition();
        position = new Vector3(position.x, spawnPosY);
        
        Transform tileGridVisualTransform = Instantiate(prefabVisualNode, position, Quaternion.identity);
        tileGridVisualTransform.GetComponentInChildren<SpriteRenderer>().sprite = e.tile.TileTemp.GetSprite(e.tile.TileState);

        TileVisual tileVisual = new TileVisual(tileGridVisualTransform, e.tile);
        tileGridDictionary[e.tile] = tileVisual;
    }

    void RemoveTileFromDictionary(object sender, EventArgs e)
    {
        if (sender is TilePosition tilePosition && tilePosition.Tile != null)
        {
            tileGridDictionary.Remove(tilePosition.Tile);
        }
    }

    private void UpdateVisualsPositions()
    {
        foreach (Tile tile in tileGridDictionary.Keys)
        {
            tileGridDictionary[tile].Update();          
        }
    }

    public void UpdateVisuals()
    {
        foreach (Tile tile in tileGridDictionary.Keys)
        {
            tileGridDictionary[tile].UpdateSprite(tile.TileTemp.GetSprite(tile.TileState));           
        }
    }
}