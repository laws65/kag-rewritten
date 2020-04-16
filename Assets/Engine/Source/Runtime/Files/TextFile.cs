using System.Text;

namespace KAG.Runtime.Modules
{
    public class TextFile : File
    {
        public string Text { get; } = "";

        public TextFile(GameModule gameModule, byte[] buffer) : base(gameModule)
        {
            Text = Encoding.UTF8.GetString(buffer);
        }
    }
}
