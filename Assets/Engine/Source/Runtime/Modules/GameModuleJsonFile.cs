using Jint.Native;

namespace KAG.Runtime.Modules
{
    public class GameModuleJsonFile : GameModuleTextFile
    {
        public JsValue Value
        {
            get
            {
                return module.jsonParser.Parse(Text);
            }
        }

        public GameModuleJsonFile(GameModule gameModule, byte[] buffer) : base(gameModule, buffer) { }
    }
}
