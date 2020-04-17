using Jint.Native;

namespace KAG.Runtime.Utils
{
    using KAG.Runtime.Modules;
    using UnityEngine;

    public class EngineUtils : BaseUtils
    {
        public float deltaTime { get { return Time.deltaTime; } }
        public float fixedDeltaTime { get { return Time.fixedDeltaTime; } }

        public EngineUtils(GameModule gameModule) : base(gameModule)
        {
            module.SetGlobalObject("Engine", this);
        }

        public void Instantiate(JsValue controller)
        {
            GameBehaviour obj = Object.Instantiate(new GameObject()).AddComponent<GameBehaviour>();
            obj.controller = controller;
        }

        public void Include(string filePath)
        {
            module.Execute(filePath);
        }
    }
}
