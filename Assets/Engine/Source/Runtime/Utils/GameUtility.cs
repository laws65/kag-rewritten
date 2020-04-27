using System.IO;
using Jint;
using Jint.Native;
using Mirror;
using UnityEngine;

namespace KAG.Runtime.Utils
{
    public class GameUtility : BaseUtility
    {
        public GameUtility(GameRuntime gameRuntime) : base(gameRuntime)
        {
            gameRuntime.SetObject("Game", this);
        }
    }

    public class BaseUtility
    {
        protected GameRuntime gameRuntime;

        public BaseUtility(GameRuntime gameRuntime)
        {
            this.gameRuntime = gameRuntime;
        }

        public JsValue ToValue(object obj)
        {
            return JsValue.FromObject(gameRuntime.jint, obj);
        }

        public T ToObject<T>(JsValue val)
        {
            return (T)val.ToObject();
        }
    }
}
