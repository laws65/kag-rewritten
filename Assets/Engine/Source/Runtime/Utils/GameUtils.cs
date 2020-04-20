using Jint;
using Jint.Native;
using UnityEngine;

namespace KAG.Runtime.Utils
{
    using Jint.Native.Object;
    using KAG.Runtime.Modules;

    public class GameUtils : BaseUtils
    {
        public GameUtils(GameModule gameModule) : base(gameModule)
        {
            module.SetObject("Game", this);
        }

        public void Instantiate(JsValue controller)
        {
            GameBehaviour obj = new GameObject("GameBehaviour").AddComponent<GameBehaviour>();
            obj.controller = controller;
        }

        public void Instantiate(string name, JsValue controller)
        {
            GameBehaviour obj = new GameObject(name).AddComponent<GameBehaviour>();
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
