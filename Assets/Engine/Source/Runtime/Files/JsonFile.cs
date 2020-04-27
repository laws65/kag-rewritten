using Jint.Native;

namespace KAG.Runtime
{
    public class JsonFile : TextFile
    {
        public JsValue Value
        {
            get
            {
                return gameRuntime.jsonParser.Parse(Text);
            }
        }

        public JsonFile(GameRuntime gameRuntime, byte[] buffer) : base(gameRuntime, buffer) { }
    }
}
