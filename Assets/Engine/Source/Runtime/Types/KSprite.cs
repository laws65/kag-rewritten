using System;
using System.Collections.Generic;
using UnityEngine;

namespace KAG.Runtime.Types
{
    [Serializable]
    public class KSprite : KType
    {
        [NonSerialized]
        public Sprite sprite;

        public string name;
        public KTexture texture;

        public int frame = 0;
        public int width = 0;
        public int height = 0;

        public KVector2 pivot;
        public List<KVector2> geometry;

        public KSprite(string path)
        {
            texture = new KTexture(path);
        }

        public void SetFrame(int value)
        {
            frame = value;

            int x = frame % (texture.width / width) * width;
            int y = frame / (texture.width / width) * height;
            sprite = Sprite.Create(texture.texture, new Rect(x, y, width, height), new Vector2(pivot.x, pivot.y));
        }

        public void SetSize(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public void SetSize(KVector2 size)
        {
            SetSize(Mathf.RoundToInt(size.x), Mathf.RoundToInt(size.y));
        }

        public void SetGeometry(List<KVector2> vertices)
        {
            geometry = vertices;
        }

        public List<KVector2> GetGeometry()
        {
            return geometry;
        }

        public void Refresh()
        {
            texture.Refresh();

            SetSize(width, height);
            SetGeometry(geometry);
            SetFrame(frame);
        }
    }
}
