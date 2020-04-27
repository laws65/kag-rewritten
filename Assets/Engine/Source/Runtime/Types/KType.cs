using System;
using TinyJSON;

namespace KAG.Runtime.Types
{
    [Serializable]
    public class KType
    {
        protected static GameRuntime gameRuntime
        {
            get
            {
                return GameEngine.Instance.gameRuntime;
            }
        }
    }
}
