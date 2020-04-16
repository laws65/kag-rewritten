using UnityEngine;

namespace KAG.Runtime.Modules
{
    public class TextureFile : File
    {
        public Texture2D texture = new Texture2D(1, 1);

        public TextureFile(GameModule gameModule, byte[] buffer) : base(gameModule)
        {
            texture.filterMode = FilterMode.Point;
            ImageConversion.LoadImage(texture, buffer);
        }
    }
}
