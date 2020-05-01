using Jint.Native;

namespace KAG.Runtime
{
    public class GameMain
    {
        protected GameEngine engine;
        public JsValue controller;

        public GameMain(GameManager manager)
        {
            engine = manager.engine;
            controller = engine.FromClass("Main");
        }

        public void Start(string gameMode)
        {
            Call("Start", gameMode);
        }

        public void OnPlayerConnected(PlayerInfo player)
        {
            Call("OnPlayerConnected", player);
        }

        public void OnPlayerDisconnected(PlayerInfo player)
        {
            Call("OnPlayerConnected", player);
        }

        public JsValue GetValue(string property)
        {
            return controller.AsObject().Get(property);
        }

        public void SetValue(string property, JsValue value)
        {
            controller.AsObject().Set(property, value);
        }

        public void Call(string function, params object[] arguments)
        {
            if (controller != null)
            {
                if (engine.GetValue(controller, function).IsUndefined())
                {
                    return;
                }

                engine.Invoke(controller.Get(function), controller, arguments);
            }
        }
    }
}
