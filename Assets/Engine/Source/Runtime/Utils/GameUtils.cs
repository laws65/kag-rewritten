using Jint;
using Jint.Native;

namespace KAG.Runtime
{
    public class GameUtils : BaseUtils
    {
        public GameUtils(GameModule gameModule) : base(gameModule)
        {
            module.SetGlobalObject("GameUtils", this);
        }

        public JsValue ParseJSON(string filePath)
        {
            return null;
        }
    }

    public class BaseUtils
    {
        protected GameModule module;

        public BaseUtils(GameModule gameModule)
        {
            module = gameModule;
        }
    }
}
