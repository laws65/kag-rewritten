using Mirror;
using Jint;
using Jint.Native;
using UnityEngine;

namespace KAG.Runtime
{
    public class GameBehaviour : NetworkBehaviour
    {
        public JsValue controller;

        private void Start()
        {
            SetValue("isMine", isLocalPlayer);
            SetValue("isClient", isClient);
            SetValue("isServer", isServer);

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
                GameRuntime.Instance.Engine.Invoke(controller.Get(functionName), controller, arguments);
            }
        }
    }
}
