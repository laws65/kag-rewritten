using UnityEngine;

namespace KAG.Runtime.Modules
{
    public class GameModuleTextureFile : GameModuleFile
    {
        public Texture2D texture = new Texture2D(1, 1);

        public GameModuleTextureFile(GameModule gameModule, byte[] buffer) : base(gameModule)
        {
            ImageConversion.LoadImage(texture, buffer);
        }
    }
}
