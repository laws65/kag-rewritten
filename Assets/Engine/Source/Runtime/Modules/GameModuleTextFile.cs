using System.Text;

namespace KAG.Runtime.Modules
{
    public class GameModuleTextFile : GameModuleFile
    {
        public string Text { get; } = "";

        public GameModuleTextFile(GameModule gameModule, byte[] buffer) : base(gameModule)
        {
            Text = Encoding.UTF8.GetString(buffer);
        }
    }
}
