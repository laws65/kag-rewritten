using Jint;
using Jint.Native;

namespace KAG.Runtime.Utils
{
    public class GameUtility : BaseUtility
    {
        public GameUtility(GameEngine engine) : base(engine)
        {
            engine.SetObject("Game", this);
        }
    }

    public class BaseUtility
    {
        protected GameEngine engine;

        public BaseUtility(GameEngine engine)
        {
            this.engine = engine;
        }

        public JsValue ToValue(object obj)
        {
            return JsValue.FromObject(engine, obj);
        }

        public T ToObject<T>(JsValue val)
        {
            return (T)val.ToObject();
        }
    }
}
