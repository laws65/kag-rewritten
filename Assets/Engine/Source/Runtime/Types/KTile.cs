using System;
using TinyJSON;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace KAG.Runtime.Types
{
    [Serializable]
    public class KTile : KType
    {
        [Exclude]
        public KTileBase tile = ScriptableObject.CreateInstance<KTileBase>();

        public string name = "";
        public string color = "";
        public KSprite[] sprites;

        public KSprite GetSprite(string name)
        {
            foreach (var sprite in sprites)
            {
                if (sprite.name == name)
                {
                    return sprite;
                }
            }

            return null;
        }

        public void SetSprite(string name)
        {
            var sprite = GetSprite(name);

            if (sprite != null)
            {
                tile.sprite = sprite.sprite;
            }
        }

        [AfterDecode]
        public void Refresh()
        {
            SetSprite("default");
        }

        public static KTile FromFile(string path)
        {
            var json = engine.Get<JsonFile>(path).Text;
            return JSON.Load(json).Make<KTile>();
        }
    }

    public class KTileBase : Tile
    {
        public void Awake()
        {
            colliderType = ColliderType.Sprite;
        }
    }
}
