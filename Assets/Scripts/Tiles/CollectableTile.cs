using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CollectableTile", menuName = "Tiles/New collectable tile")]
public class CollectableTile : TileTemplate
{
    [SerializeField] Sprite spriteA;
    [SerializeField] Sprite spriteB;
    [SerializeField] Sprite spriteC;

    public Sprite SpriteA { get => spriteA; set => spriteA = value; }
    public Sprite SpriteB { get => spriteB; set => spriteB = value; }
    public Sprite SpriteC { get => spriteC; set => spriteC = value; }

    public override void Behaviour()
    {
        throw new System.NotImplementedException();
    }

    public override Sprite GetSprite(string name)
    {
        if(name == "A")
        {
            return spriteA;
        }
        else if(name == "B")
        {
            return spriteB;
        }
        else if(name == "C")
        {
            return spriteC;
        }else
            return base.GetSprite(name);
    }
}
