using UnityEngine;
using UnityEngine.Assertions;

namespace KAG.Runtime.Utils
{
    public class AssertUtility : BaseUtility
    {
        public AssertUtility(GameRuntime gameRuntime) : base(gameRuntime)
        {
            gameRuntime.SetObject("Assert", this);
        }

        public void AreEqual(object expected, object actual)
        {
            Assert.AreEqual(expected, actual);
        }
    }
}
