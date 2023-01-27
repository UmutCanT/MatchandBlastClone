using UnityEngine;

public class TileObject: MonoBehaviour
{
    [SerializeField]SpriteRenderer spriteRenderer;  

    public void ChangeSpriteSize(Vector2 size)
    {
        spriteRenderer.size = size;
    }

    public void ChangeSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }
}