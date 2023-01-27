using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CollectableTile", menuName = "Tiles/New collectable tile")]
public class CollectableTile : TileTemplate
{
    [SerializeField] List<Sprite> bonusSprites;

    public List<Sprite> BonusSprites { get => bonusSprites; }

    public override void Behaviour()
    {
        throw new System.NotImplementedException();
    }
}
