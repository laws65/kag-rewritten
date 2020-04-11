using UnityEngine;
using UnityEngine.Assertions;
using Jint;
using Jint.Native;

namespace KAG.Runtime.Utils
{
    using KAG.Runtime.Modules;

    public class AssertUtils : BaseUtils
    {
        public AssertUtils(GameModule gameModule) : base(gameModule)
        {
            module.SetGlobalObject("Assert", this);
        }

        public void AreEqual(object expected, object actual)
        {
            Assert.AreEqual(expected, actual);
        }
    }
}
