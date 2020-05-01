using System;
using TinyJSON;
using UnityEngine;

namespace KAG.Runtime.Types
{
    [Serializable]
    public class KTexture : KType
    {
        [Exclude]
        public Texture2D texture = new Texture2D(1, 1);

        public string file;
        public KVector2Int size;

        public KColor[] GetPixels()
        {
            Color32[] original = texture.GetPixels32();
            KColor[] pixels = new KColor[size.x * size.y];

            for (int i = 0; i < pixels.Length; ++i)
            {
                pixels[i] = new KColor(original[i]);
            }

            return pixels;
        }

        [AfterDecode]
        public void Refresh()
        {
            texture = engine.Get<TextureFile>(file).texture;

            size = new KVector2Int(texture.width, texture.height);
        }

        public static KTexture FromFile(string path)
        {
            KTexture texture = new KTexture();
            texture.file = path;
            texture.Refresh();

            return texture;
        }
    }
}
