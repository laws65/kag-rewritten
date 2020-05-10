using Mirror;
using Jint;
using Jint.Native;

namespace KAG.Runtime
{
    public class GameEntity : NetworkBehaviour
    {
        protected GameEngine engine => GameManager.Instance.engine;
        protected JsValue controller;

        [SyncVar]
        public string classPath = "";

        public void Init(string classPath)
        {
            this.classPath = classPath;
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
        }

        protected virtual void Start()
        {
            controller = engine.FromClass(engine.Import(classPath));

            controller.SetValue(engine, "isMine", isLocalPlayer);
            controller.SetValue(engine, "isClient", isClient);
            controller.SetValue(engine, "isServer", isServer);

            controller.Call(engine, "Start");
        }

        protected virtual void Update()
        {
            controller.Call(engine, "Update");
        }
    }

    public static class GameEntityExtensions
    {
        public static JsValue GetValue(this JsValue controller, GameEngine engine, string property)
        {
            return controller.AsObject().Get(property);
        }

        public static void SetValue(this JsValue controller, GameEngine engine, string property, JsValue value)
        {
            controller.AsObject().Set(property, value);
        }

        public static void Call(this JsValue controller, GameEngine engine, string function, params object[] args)
        {
            if (engine.GetValue(controller, function).IsUndefined()) return;

            engine.Invoke(controller.Get(function), controller, args);
        }
    }
}
