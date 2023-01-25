using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TileTemplate : ScriptableObject
{
    [SerializeField] string tileName;
    [SerializeField] TileTypes tileType;
    [SerializeField] Sprite tileSprite;

    public abstract void Behaviour();
}
