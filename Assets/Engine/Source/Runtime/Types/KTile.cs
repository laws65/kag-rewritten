using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace KAG.Runtime.Types
{
    [Serializable]
    public class KTile
    {
        [NonSerialized]
        public Tile tile;

        public KTile()
        {
            tile = ScriptableObject.CreateInstance<Tile>();
        }

        public void SetSprite(KSprite sprite)
        {
            tile.sprite = Sprite.Create(sprite.texture, new Rect(0, 0, sprite.Width, sprite.Height), new Vector2(0.5f, 0.5f));
        }
    }
}
