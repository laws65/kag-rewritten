using UnityEngine;

namespace KAG.Runtime.Utils
{
    public class DebugUtility : BaseUtility
    {
        public DebugUtility(GameRuntime gameRuntime) : base(gameRuntime)
        {
            gameRuntime.SetObject("Debug", this);
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
