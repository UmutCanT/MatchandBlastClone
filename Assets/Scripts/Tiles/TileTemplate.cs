using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TileTemplate : ScriptableObject
{
    [SerializeField] Sprite tileSprite;
    
    public virtual Sprite GetSprite(TileStates tileState)
    {
        return tileSprite;
    }
    public abstract void Behaviour();
}
