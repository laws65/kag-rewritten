using UnityEngine;

namespace KAG.Runtime
{
    public class TextureFile : File
    {
        public Texture2D texture = new Texture2D(1, 1);

        public TextureFile(GameEngine engine, byte[] buffer) : base(engine)
        {
            texture.filterMode = FilterMode.Point;
            ImageConversion.LoadImage(texture, buffer);
        }
    }
}
