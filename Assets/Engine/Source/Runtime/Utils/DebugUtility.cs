using UnityEngine;

namespace KAG.Runtime.Utils
{
    public class DebugUtility : BaseUtility
    {
        public DebugUtility(GameEngine engine) : base(engine)
        {
            engine.SetObject("Debug", this);
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
