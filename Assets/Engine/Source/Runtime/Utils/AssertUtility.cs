using UnityEngine;
using UnityEngine.Assertions;

namespace KAG.Runtime.Utils
{
    public class AssertUtility : BaseUtility
    {
        public AssertUtility(GameEngine engine) : base(engine)
        {
            engine.SetObject("Assert", this);
        }

        public void AreEqual(object expected, object actual)
        {
            Assert.AreEqual(expected, actual);
        }
    }
}
