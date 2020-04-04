using Jint;
using Jint.Native;
using Jint.Native.Json;
using Nakama.TinyJson;

namespace KAG.Runtime.Utils
{
    public class GameUtils : BaseUtils
    {
        public GameUtils(GameModule gameModule) : base(gameModule)
        {
            module.SetGlobalObject("Utils", this);
        }

        public object FromJson(string filePath)
        {
            GameModuleJsonFile file = module.Get<GameModuleJsonFile>(filePath);
            return file?.Value;
        }

        public string ToJson(object value)
        {
            return value.ToJson();
        }

        public object GetSprite(string filePath)
        {
            return module.Get<GameModuleSpriteFile>(filePath).Sprite;
        }
    }

    public class BaseUtils
    {
        protected GameModule module;

        public BaseUtils(GameModule gameModule)
        {
            module = gameModule;
        }

        public JsValue ToValue(object obj)
        {
            return JsValue.FromObject(module.engine, obj);
        }

        public T ToObject<T>(JsValue val)
        {
            return (T)val.ToObject();
        }
    }
}
