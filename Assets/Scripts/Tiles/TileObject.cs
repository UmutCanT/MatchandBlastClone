using UnityEngine;

public class TileObject: MonoBehaviour
{
    [SerializeField]SpriteRenderer spriteRenderer;  

    public void ChangeSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }
}