using UnityEngine;

namespace KAG.Runtime
{
    public class GameDebug : BaseUtils
    {
        public GameDebug(GameModule gameModule) : base(gameModule)
        {
            module.SetGlobalObject("GameDebug", this);
        }

        public void Log(object message)
        {
            Debug.Log(message);
        }

        public void LogError(object message)
        {
            Debug.LogError(message);
        }

        public void LogWarning(object message)
        {
            Debug.LogWarning(message);
        }
    }
}
