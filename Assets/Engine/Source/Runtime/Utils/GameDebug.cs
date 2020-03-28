using UnityEngine;

namespace KAG.Runtime
{
    public class GameDebug : BaseUtils
    {
        public GameDebug(GameModule gameModule) : base(gameModule)
        {
            module.SetGlobalObject("GameDebug", this);
        }

        public void Log(string text)
        {
            Debug.Log(text);
        }
    }
}
