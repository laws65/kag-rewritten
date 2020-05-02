using TinyJSON;
using UnityEngine;

namespace KAG.Runtime.Utils
{
    public class DebugUtility : Utility
    {
        public DebugUtility(GameEngine engine) : base(engine)
        {
            engine.SetObject("Debug", this);
        }

        public void Dump(object data)
        {
            Debug.Log(JSON.Dump(data));
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
