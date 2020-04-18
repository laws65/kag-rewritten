using TinyJSON;
using Jint;
using Jint.Native;
using UnityEngine;

namespace KAG.Runtime.Utils
{
    using KAG.Runtime.Modules;

    public class GameUtils : BaseUtils
    {
        public GameUtils(GameModule gameModule) : base(gameModule)
        {
            module.SetObject("Game", this);
        }

        public void Instantiate(JsValue controller)
        {
            GameBehaviour obj = Object.Instantiate(new GameObject()).AddComponent<GameBehaviour>();
            obj.controller = controller;
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
