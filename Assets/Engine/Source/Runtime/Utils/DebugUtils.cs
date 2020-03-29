using UnityEngine;
using Jint.Native;

namespace KAG.Runtime.Utils
{
    public class DebugUtils : BaseUtils
    {
        public DebugUtils(GameModule gameModule) : base(gameModule)
        {
            module.SetGlobalObject("Debug", this);
        }

        public void Log(JsValue message)
        {
            Debug.Log(module.ToJson(message));
        }

        public void LogError(JsValue message)
        {
            Debug.LogError(module.ToJson(message));
        }

        public void LogWarning(JsValue message)
        {
            Debug.LogWarning(module.ToJson(message));
        }
    }
}
