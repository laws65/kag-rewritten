using System;
using UnityEngine;

namespace KAG.Runtime.Types
{
    [Serializable]
    public class KSprite
    {
        [NonSerialized]
        public Texture2D texture = new Texture2D(1, 1);

        public int Width { get => texture.width; }
        public int Height { get => texture.height; }

        public KSprite(byte[] buffer)
        {
            texture.filterMode = FilterMode.Point;
            ImageConversion.LoadImage(texture, buffer);
        }

        public KColor[] GetPixels()
        {
            Color32[] original = texture.GetPixels32();
            KColor[] pixels = new KColor[Width * Height];

            for (int i = 0; i < pixels.Length; ++i)
            {
                pixels[i] = new KColor(original[i]);
            }

            return pixels;
        }
    }
}
