using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace KAG.Runtime.Types
{
    using KAG.Runtime.Modules;

    public class KTile : KType
    {
        [NonSerialized]
        public KTileBase tile;

        public string name = "";
        public string color = "";

        public List<KSprite> sprites;

        public KTile(string path)
        {
            tile = ScriptableObject.CreateInstance<KTileBase>();

            var json = runtime.module.Get<GameModuleJsonFile>(path).Text;
            JsonUtility.FromJsonOverwrite(json, this);

            sprites[0].Refresh();

            tile.sprite = sprites[0].sprite;
            tile.colliderType = Tile.ColliderType.Sprite;
            tile.sprite.OverrideGeometry(sprites[0].GetGeometry().ToNative().ToArray(), tile.sprite.triangles);
        }
    }

    public class KTileBase : Tile
    {

    }
}
