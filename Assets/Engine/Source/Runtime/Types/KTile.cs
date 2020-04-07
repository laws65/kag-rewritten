using Boo.Lang;
using Jint.Native;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace KAG.Runtime.Types
{
    public class KTileBase : Tile
    {

    }

    public class KTile
    {
        [NonSerialized]
        public KTileBase tile;

        [NonSerialized]
        public KSprite sprite;

        [NonSerialized]
        public Vector2 pivot = new Vector2(0.5f, 0.5f);

        public int Width = 1;
        public int Height = 1;

        public KTile()
        {
            tile = ScriptableObject.CreateInstance<KTileBase>();
        }

        public void SetSprite(KSprite sprite)
        {
            this.sprite = sprite;
        }

        public void SetFrame(int frame)
        {
            int x = (frame % (sprite.Width / Width)) * Width;
            int y = (frame / (sprite.Width / Width)) * Height;
            tile.sprite = Sprite.Create(sprite.texture, new Rect(x, y, Width, Height), pivot);
        }

        public void SetGeometry(KVector[] vertices)
        {
            List<Vector2> arr = new List<Vector2>();
            foreach (var vec in vertices)
            {
                arr.Push(new Vector2(vec.x * Width, vec.y * Height));
            }

            tile.colliderType = Tile.ColliderType.Sprite;
            tile.sprite.OverrideGeometry(arr.ToArray(), tile.sprite.triangles);
        }

        public void SetSize(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public void SetPivot(float x, float y)
        {
            pivot = new Vector2(x, y);
        }
    }
}
