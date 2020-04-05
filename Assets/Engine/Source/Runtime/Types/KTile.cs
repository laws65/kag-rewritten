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

        [NonSerialized]
        public KSprite sprite;

        public int Width = 1;
        public int Height = 1;

        public KTile()
        {
            tile = ScriptableObject.CreateInstance<Tile>();
        }

        public void SetSprite(KSprite sprite)
        {
            this.sprite = sprite;
        }

        public void SetFrame(int frame)
        {
            int x = (frame % (sprite.Width / Width)) * Width;
            int y = (frame / (sprite.Width / Width)) * Height;
            tile.sprite = Sprite.Create(sprite.texture, new Rect(x, y, Width, Height), new Vector2(0.5f, 0.5f));
        }

        public void SetSize(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}
