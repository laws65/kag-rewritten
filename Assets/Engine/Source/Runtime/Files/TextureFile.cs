using UnityEngine;

namespace KAG.Runtime
{
    public class TextureFile : File
    {
        public Texture2D texture = new Texture2D(1, 1);

        public TextureFile(GameRuntime gameRuntime, byte[] buffer) : base(gameRuntime)
        {
            texture.filterMode = FilterMode.Point;
            ImageConversion.LoadImage(texture, buffer);
        }
    }
}
