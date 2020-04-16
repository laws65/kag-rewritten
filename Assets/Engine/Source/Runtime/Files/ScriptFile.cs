using Jint;

namespace KAG.Runtime.Modules
{
    public class ScriptFile : TextFile
    {
        public ScriptFile(GameModule gameModule, byte[] buffer) : base(gameModule, buffer) { }
    }
}
