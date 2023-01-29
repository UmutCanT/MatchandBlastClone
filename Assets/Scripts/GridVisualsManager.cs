using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridVisualsManager : MonoBehaviour
{
    [SerializeField] List<TileTemplate> tileTemplates;
    [SerializeField] Transform prefabVisualNode;
    [SerializeField] Map map;
    CamManager camManager;
    Grid<TilePosition> grid;
    private Dictionary<Tile, TileVisual> tileGridDictionary;

    private void Awake()
    {
        map.OnLevelSet += InitializeVisuals;
        camManager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CamManager>();
    }

    private void InitializeVisuals(object sender, Map.OnLevelSetEventArgs e)
    {
        Initialize(sender as Map, e.tilePositionGrid);
    }

    public void Initialize(Map map, Grid<TilePosition> grid)
    {
        this.map = map;
        this.grid = grid;

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
                position = new Vector3(position.x, 12);

                Transform tileGridVisualTransform = Instantiate(prefabVisualNode, position, Quaternion.identity);
                tileGridVisualTransform.GetComponentInChildren<SpriteRenderer>().sprite = tile.TileTemp.GetSprite(tile.TileState);

                TileVisual tileVisual = new TileVisual(tileGridVisualTransform, tile);

                tileGridDictionary[tile] = tileVisual;

                // Background Grid Visual
                //Instantiate(pfBackgroundGridVisual, grid.GetWorldPosition(x, y), Quaternion.identity);
            }
        }
        //UpdateVisuals();
    }

    private void InstantiateVisual(object sender, Map.OnNewGemGridSpawnedEventArgs e)
    {
        Vector3 position = e.tilePosition.GetWorldPosition();
        position = new Vector3(position.x, 12);

        Transform tileGridVisualTransform = Instantiate(prefabVisualNode, position, Quaternion.identity);
        tileGridVisualTransform.GetComponentInChildren<SpriteRenderer>().sprite = e.tile.TileTemp.GetSprite(e.tile.TileState);

        TileVisual tileVisual = new TileVisual(tileGridVisualTransform, e.tile);
    }

    private void RemoveTileFromDictionary(object sender, EventArgs e)
    {
        TilePosition tilePosition = sender as TilePosition;
        if (tilePosition != null && tilePosition.Tile != null)
        {
            tileGridDictionary.Remove(tilePosition.Tile);
        }
    }

    private void Update()
    {
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        foreach (Tile tile in tileGridDictionary.Keys)
        {
            tileGridDictionary[tile].Update();
            //StartCoroutine(tileGridDictionary[tile].UpdateMov());
        }
    }

    private class TileVisual
    {
        private Transform transform;
        private Tile tile;

        public TileVisual(Transform transform, Tile tile)
        {
            this.transform = transform;
            this.tile = tile;

            tile.OnPopped += DestroyTileVisual;
        }

        private void DestroyTileVisual(object sender, System.EventArgs e)
        {
            //transform.GetComponent<Animation>().Play();
            Destroy(transform.gameObject, 1f);
        }

        public void Update()
        {
            Vector3 targetPos = tile.GetWorldPosition();
            Vector3 moveDir  = targetPos - transform.position;
            float moveSpeed = 2f;
            transform.position += moveDir * moveSpeed * Time.deltaTime;
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