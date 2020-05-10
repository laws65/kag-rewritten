using Jint.Native;

namespace KAG.Runtime
{
    public class ScriptFile : TextFile
    {
        public bool imported = false;
        public JsValue importedValue = JsValue.Undefined;

        public ScriptFile(GameEngine engine, byte[] buffer) : base(engine, buffer) { }
    }
}
