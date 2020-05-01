using System.Text;

namespace KAG.Runtime
{
    public class TextFile : File
    {
        public string Text { get; protected set; } = "";

        public TextFile(GameEngine engine, byte[] buffer) : base(engine)
        {
            Text = Encoding.UTF8.GetString(buffer);
        }
    }
}
