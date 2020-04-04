using System.Text;
using UnityEngine;

namespace KAG.Runtime
{
    using KAG.Runtime.Types;

    public class GameModuleSpriteFile : GameModuleFile
    {
        public KSprite Sprite { get; }

        public GameModuleSpriteFile(GameModule gameModule, byte[] buffer) : base(gameModule)
        {
            Sprite = new KSprite(buffer);
        }
    }
}
