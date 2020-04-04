using UnityEngine;
using Nakama.TinyJson;

namespace KAG.Runtime.Utils
{
    public class DebugUtils : BaseUtils
    {
        public DebugUtils(GameModule gameModule) : base(gameModule)
        {
            module.SetGlobalObject("Debug", this);
        }

        public void Log(object message)
        {
            Debug.Log(message.ToJson());
        }

        public void LogError(object message)
        {
            Debug.LogError(message.ToJson());
        }

        public void LogWarning(object message)
        {
            Debug.LogWarning(message.ToJson());
        }
    }
}
