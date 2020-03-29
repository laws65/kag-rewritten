using UnityEngine;
using UnityEngine.Assertions;
using Jint;
using Jint.Native;

namespace KAG.Runtime.Utils
{
    public class AssertUtils : BaseUtils
    {
        public AssertUtils(GameModule gameModule) : base(gameModule)
        {
            module.SetGlobalObject("Assert", this);
        }

        public void AreEqual(JsValue expected, JsValue actual)
        {
            Assert.AreEqual(module.ToJson(expected), module.ToJson(actual));
        }
    }
}
