using System;
using UnityEngine;

public partial class GridVisualsManager
{
    public class TileVisual
    {
        private Transform transform;
        private Tile tile;
        readonly float moveSpeed = 2.2f;

        public TileVisual(Transform transform, Tile tile)
        {
            this.transform = transform;
            this.tile = tile;

            tile.OnPopped += DestroyTileVisual;
        }

        private void DestroyTileVisual(object sender, EventArgs e)
        {
            Destroy(transform.gameObject);
        }

        public void UpdateSprite(Sprite sprite)
        {
            transform.GetComponentInChildren<SpriteRenderer>().sprite = sprite;
        }

        public void Update()
        {
            Vector3 targetPos = tile.GetWorldPosition();
            Vector3 moveDir  = targetPos - transform.position;           
            transform.position += moveSpeed * Time.deltaTime * moveDir;
        }
    }
}