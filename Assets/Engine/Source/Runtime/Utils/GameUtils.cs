using UnityEngine;
using Jint;
using Jint.Native;

namespace KAG.Runtime.Utils
{
    using KAG.Runtime.Modules;

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

        public string ToJson(object value, bool prettify = false)
        {
            return JsonUtility.ToJson(value, prettify);
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
