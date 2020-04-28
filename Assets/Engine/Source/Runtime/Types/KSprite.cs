using System;
using TinyJSON;
using UnityEngine;

namespace KAG.Runtime.Types
{
    [Serializable]
    public class KSprite : KType
    {
        [Exclude]
        public Sprite sprite;

        public int frame;
        public string name;
        public KTexture texture;
        public KVector2Int size;

        public KVector2[] geometry;
        public KVector2 pivot = new KVector2(0.5f, 0.5f);

        [AfterDecode]
        public void Refresh()
        {
            int x = frame % (texture.size.x / size.x) * size.x;
            int y = frame / (texture.size.x / size.x) * size.y;

            sprite = Sprite.Create(texture.texture, new Rect(x, y, size.x, size.y), pivot.ToNative());

            if (geometry != null)
            {
                sprite.OverrideGeometry(geometry.ToNative().ToArray(), sprite.triangles);
            }
        }
    }
}
