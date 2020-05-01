using Jint.Native;

namespace KAG.Runtime
{
    public class JsonFile : TextFile
    {
        public JsValue Value
        {
            get
            {
                return engine.jsonParser.Parse(Text);
            }
        }

        public JsonFile(GameEngine engine, byte[] buffer) : base(engine, buffer) { }
    }
}
