using UnityEngine;
using UnityEngine.Tilemaps;

namespace KAG.Runtime.Utils
{
    using KAG.Runtime.Types;

    public class MapUtils : BaseUtils
    {
        public Grid grid;
        public Tilemap tilemap;
        public TilemapRenderer tilemapRenderer;

        public MapUtils(GameModule gameModule) : base(gameModule)
        {
            module.SetGlobalObject("Map", this);

            grid = new GameObject("Grid").AddComponent<Grid>();
            tilemap = new GameObject("Tilemap").AddComponent<Tilemap>();
            tilemapRenderer = tilemap.gameObject.AddComponent<TilemapRenderer>();
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
