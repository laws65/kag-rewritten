using UnityEngine;
using UnityEngine.Tilemaps;

namespace KAG.Runtime.Utils
{
    using KAG.Runtime.Types;

    public class MapUtility : Utility
    {
        public Grid grid;
        public Tilemap tilemap;
        public TilemapRenderer tilemapRenderer;
        public TilemapCollider2D tilemapCollider;
        public Rigidbody2D tilemapRigidbody;
        public CompositeCollider2D tilemapComposite;

        public MapUtility(GameEngine engine) : base(engine)
        {
            engine.SetObject("Map", this);

            grid = new GameObject("Grid").AddComponent<Grid>();
            tilemap = new GameObject("Tilemap").AddComponent<Tilemap>();
            tilemapRenderer = tilemap.gameObject.AddComponent<TilemapRenderer>();
            tilemapCollider = tilemap.gameObject.AddComponent<TilemapCollider2D>();
            tilemapComposite = tilemap.gameObject.AddComponent<CompositeCollider2D>();
            tilemapRigidbody = tilemap.gameObject.GetComponent<Rigidbody2D>();

            tilemapRigidbody.bodyType = RigidbodyType2D.Static;
            tilemapCollider.usedByComposite = true;

            tilemap.transform.SetParent(grid.transform);
        }

        public void SetTile(int x, int y, KTile tile)
        {
            tilemap.SetTile(new Vector3Int(x, y, 0), tile.tile);
        }

        public void SetTileSize(float width, float height)
        {
            grid.cellSize = new Vector3(width / 100f, height / 100f);
        }
    }
}
