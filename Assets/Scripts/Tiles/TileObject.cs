using UnityEngine;
using UnityEngine.Pool;

public class TileObject: MonoBehaviour
{
    [SerializeField]SpriteRenderer spriteRenderer;  

    public void ChangeSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }

    public void ChangeSprite()
    {
        spriteRenderer.sprite = null;
    }

    private IObjectPool<TileObject> tileObjectPool;

    public void SetPool(IObjectPool<TileObject> pool)
    {
        tileObjectPool = pool;
    }

    void OnPopUp()
    {
        tileObjectPool.Release(this);
    }
}