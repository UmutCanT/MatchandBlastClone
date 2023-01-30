using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridVisualsManager : MonoBehaviour
{
    [SerializeField] List<TileTemplate> tileTemplates;
    [SerializeField] Transform prefabVisualNode;
    [SerializeField] Map map;
    CamManager camManager;
    private Dictionary<Tile, TileVisual> tileGridDictionary;
    float spawnPosY;

    private void Awake()
    {
        map.OnLevelSet += InitializeVisuals;
        camManager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CamManager>();
    }


    private void OnEnable()
    {
        map.OnCheckFinish += UpdateVisuals;
    }

    private void OnDisable()
    {
        map.OnCheckFinish -= UpdateVisuals;
    }

    private void InitializeVisuals(object sender, Map.OnLevelSetEventArgs e)
    {
        Initialize(sender as Map, e.tilePositionGrid);
    }

    public void Initialize(Map map, Grid<TilePosition> grid)
    {
        this.map = map;
        spawnPosY = grid.Height + .5f;

        camManager.changeCamPosition(grid.Width, grid.Height);
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

    private void InstantiateVisual(object sender, Map.OnNewGemGridSpawnedEventArgs e)
    {
        Vector3 position = e.tilePosition.GetWorldPosition();
        position = new Vector3(position.x, spawnPosY);
        
        Transform tileGridVisualTransform = Instantiate(prefabVisualNode, position, Quaternion.identity);
        tileGridVisualTransform.GetComponentInChildren<SpriteRenderer>().sprite = e.tile.TileTemp.GetSprite(e.tile.TileState);

        TileVisual tileVisual = new TileVisual(tileGridVisualTransform, e.tile);
        tileGridDictionary[e.tile] = tileVisual;
    }

    private void RemoveTileFromDictionary(object sender, EventArgs e)
    {
        if (sender is TilePosition tilePosition && tilePosition.Tile != null)
        {
            tileGridDictionary.Remove(tilePosition.Tile);
        }
    }

    private void Update()
    {
        UpdateVisualsPositions();
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

    private class TileVisual
    {
        private Transform transform;
        private Tile tile;
        readonly float moveSpeed = 2.2f;

        public TileVisual(Transform transform, Tile tile)
        {
            this.transform = transform;
            this.tile = tile;

            tile.OnPopped += DestroyTileVisual;
        }

        private void DestroyTileVisual(object sender, EventArgs e)
        {
            Destroy(transform.gameObject);
        }

        public void UpdateSprite(Sprite sprite)
        {
            transform.GetComponentInChildren<SpriteRenderer>().sprite = sprite;
        }

        public void Update()
        {
            Vector3 targetPos = tile.GetWorldPosition();
            Vector3 moveDir  = targetPos - transform.position;           
            transform.position += moveSpeed * Time.deltaTime * moveDir;
        }

        public IEnumerator UpdateMov()
        {
            Vector3 targetPos = tile.GetWorldPosition();
            while (transform.position != targetPos)
            {
                Vector3 moveDir = targetPos - transform.position;
                float moveSpeed = 10f;
                transform.position += moveSpeed * Time.deltaTime * moveDir;
                yield return new WaitForSeconds(.5f);
            }
            yield return null;
        }
    }
}