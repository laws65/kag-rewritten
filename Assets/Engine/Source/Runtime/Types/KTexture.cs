using System;
using UnityEngine;

namespace KAG.Runtime.Types
{
    using KAG.Runtime.Modules;

    [Serializable]
    public class KTexture : KType
    {
        [NonSerialized]
        public Texture2D texture = new Texture2D(1, 1);

        public string file = "";
        public int width = 0;
        public int height = 0;

        public KTexture(string path)
        {
            file = path;
            Refresh();
        }

        public KColor[] GetPixels()
        {
            Color32[] original = texture.GetPixels32();
            KColor[] pixels = new KColor[width * height];

            for (int i = 0; i < pixels.Length; ++i)
            {
                pixels[i] = new KColor(original[i]);
            }

            return pixels;
        }

        public void Refresh()
        {
            var textureFile = runtime.module.Get<GameModuleTextureFile>(file);
            texture = textureFile.texture;

            width = texture.width;
            height = texture.height;
        }
    }
}
