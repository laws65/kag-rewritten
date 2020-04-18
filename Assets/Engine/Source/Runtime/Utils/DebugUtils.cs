using UnityEngine;

namespace KAG.Runtime.Utils
{
    using KAG.Runtime.Modules;

    public class DebugUtils : BaseUtils
    {
        public DebugUtils(GameModule gameModule) : base(gameModule)
        {
            module.SetObject("Debug", this);
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
