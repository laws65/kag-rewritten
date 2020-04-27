using System.Text;

namespace KAG.Runtime
{
    public class TextFile : File
    {
        public string Text { get; protected set; } = "";

        public TextFile(GameRuntime gameRuntime, byte[] buffer) : base(gameRuntime)
        {
            Text = Encoding.UTF8.GetString(buffer);
        }
    }
}
