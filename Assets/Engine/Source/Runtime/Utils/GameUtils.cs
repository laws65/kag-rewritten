using Jint;
using Jint.Native;
using Jint.Native.Json;

namespace KAG.Runtime.Utils
{
    public class GameUtils : BaseUtils
    {
        public GameUtils(GameModule gameModule) : base(gameModule)
        {
            module.SetGlobalObject("Utils", this);
        }

        public JsValue FromJson(string filePath)
        {
            var file = module.Get<GameModuleJsonFile>(filePath);

            return file?.Value ?? JsValue.Null;
        }

        public JsValue ToJson(JsValue value)
        {
            return module.ToJson(value);
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
