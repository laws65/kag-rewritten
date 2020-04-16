using Jint.Native;

namespace KAG.Runtime.Modules
{
    public class JsonFile : TextFile
    {
        public JsValue Value
        {
            get
            {
                return module.jsonParser.Parse(Text);
            }
        }

        public JsonFile(GameModule gameModule, byte[] buffer) : base(gameModule, buffer) { }
    }
}
