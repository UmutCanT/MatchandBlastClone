using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CollectableTemplate", menuName = "Tiles/New Collectable Tile")]
public class CollectableTemplate : TileTemplate
{
    [SerializeField] Sprite spriteA;
    [SerializeField] Sprite spriteB;
    [SerializeField] Sprite spriteC;
  
    public override Sprite GetSprite(TileStates tileState)
    {
        return tileState switch
        {
            TileStates.A => spriteA,
            TileStates.B => spriteB,
            TileStates.C => spriteC,           
            _ => base.GetSprite(tileState)
        };
    }
}
