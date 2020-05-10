using Jint.Native;

namespace KAG.Runtime
{
    public class GameMain
    {
        protected GameEngine engine => GameManager.Instance.engine;
        protected JsValue controller;

        public GameMain(string className)
        {
            controller = engine.FromClass(className);
        }

        public void Start(string rulesPath)
        {
            controller.Call(engine, "Start", rulesPath);
        }

        public void OnPlayerConnected(PlayerInfo player)
        {
            controller.Call(engine, "OnPlayerConnected", player);
        }

        public void OnPlayerDisconnected(PlayerInfo player)
        {
            controller.Call(engine, "OnPlayerConnected", player);
        }
    }
}
