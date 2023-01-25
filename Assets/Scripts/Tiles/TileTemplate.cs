using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TileTemplate : ScriptableObject
{
    [SerializeField] string tileName;
    [SerializeField] TileTypes tileType;
    [SerializeField] Sprite tileSprite;

    public Sprite TileSprite { get => tileSprite; set => tileSprite = value; }
    public TileTypes TileType { get => tileType; set => tileType = value; }

    public abstract void Behaviour();
}
