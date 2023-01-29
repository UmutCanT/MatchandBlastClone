using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Pool;

public class CollectableSpawner : MonoBehaviour
{
    [SerializeField] TileTemplate[] tileTemplates;
    [SerializeField] TileObject collectable;
    IObjectPool<TileObject> tileObjectPool;
    Vector3 spawnPosition;

    public Vector3 SpawnPosition { get => spawnPosition; set => spawnPosition = value; }

    private void Awake()
    {
        tileObjectPool = new ObjectPool<TileObject>(
            CreateCollectable,
            OnGet,
            OnRelease
            );
    }

    private void OnRelease(TileObject obj)
    {
        obj.gameObject.SetActive(false);
    }

    private void OnGet(TileObject obj)
    {
        
        obj.gameObject.SetActive(true);      
    }

    public void GetCollectables(Vector3 position)
    {
        tileObjectPool.Get();
    }

    private TileObject CreateCollectable()
    {
        TileObject obj = Instantiate(collectable);
        obj.SetPool(tileObjectPool);
        return obj;
    }

    void CollectableSprite(TileObject col)
    {
    }
}
