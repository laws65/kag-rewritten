using Mirror;
using Jint;
using Jint.Native;
using System.IO;
using UnityEngine;

namespace KAG.Runtime
{
    public class GameEntity : NetworkBehaviour
    {
        private GameRuntime gameRuntime;

        public JsValue controller;

        private void Start()
        {
            gameRuntime = GameEngine.Instance.gameRuntime;

            SetValue("isMine", isLocalPlayer);
            SetValue("isClient", isClient);
            SetValue("isServer", isServer);
            //SetValue("Instantiate", Instantiate);

            Call("Start");
        }

        private void Update()
        {
            Call("Update");
        }

        public void SetValue(string propertyName, JsValue value)
        {
            controller.AsObject().Set(propertyName, value);
        }

        public JsValue GetValue(string propertyName)
        {
            return controller.AsObject().Get(propertyName);
        }

        public void Call(string functionName, params JsValue[] arguments)
        {
            if (controller != null)
            {
                gameRuntime.jint.Invoke(controller.Get(functionName), controller, arguments);
            }
        }
    }
}
