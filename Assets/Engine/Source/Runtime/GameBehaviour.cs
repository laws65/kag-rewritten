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
            Call("Start");
        }

        private void Update()
        {
            Call("Update");
        }

        private void Call(string function_name, params JsValue[] arguments)
        {
            controller?.Get(function_name)?.Invoke(arguments);
        }
    }
}
