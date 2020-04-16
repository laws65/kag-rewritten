using System;
using TinyJSON;

namespace KAG.Runtime.Types
{
    [Serializable]
    public class KType
    {
        protected static GameRuntime runtime
        {
            get
            {
                return GameRuntime.Instance;
            }
        }
    }
}
