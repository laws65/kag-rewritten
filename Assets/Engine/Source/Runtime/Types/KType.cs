using System;
using TinyJSON;

namespace KAG.Runtime.Types
{
    [Serializable]
    public class KType
    {
        protected static GameEngine engine
        {
            get
            {
                return GameManager.Instance.engine;
            }
        }
    }
}
