using UnityEngine;
using UnityEngine.Assertions;
using Jint;
using Jint.Native;
using Nakama.TinyJson;

namespace KAG.Runtime.Utils
{
    public class AssertUtils : BaseUtils
    {
        public AssertUtils(GameModule gameModule) : base(gameModule)
        {
            module.SetGlobalObject("Assert", this);
        }

        public void AreEqual(object expected, object actual)
        {
            Assert.AreEqual(expected.ToJson(), actual.ToJson());
        }
    }
}
