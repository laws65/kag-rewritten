using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Text;

namespace KAG
{
    public class GameModule
    {
        public List<GameModuleFile> fileList = new List<GameModuleFile>();

        public GameModule(Stream zipStream)
        {
            ZipArchive zipArchive = new ZipArchive(zipStream);

            foreach (var zipEntry in zipArchive.Entries)
            {
                AddFile(zipEntry.FullName, zipEntry.Open());
            }
        }

        public void AddFile(string filePath, Stream fileStream)
        {
            byte[] fileBuffer;

            using (var ms = new MemoryStream())
            {
                fileStream.CopyTo(ms);
                fileBuffer = ms.ToArray();
            }

            GameModuleFile file = null;
            switch (Path.GetExtension(filePath))
            {
                case ".txt":
                    file = new GameModuleTextFile(filePath, fileBuffer);
                    break;
            }

            if (file != null)
            {
                fileList.Add(file);
            }
        }
    }

    public class GameModuleFile
    {
        public string Path { get; } = "";

        public GameModuleFile(string path)
        {
            Path = path;
        }
    }

    public class GameModuleTextFile : GameModuleFile
    {
        public string Content { get; } = "";

        public GameModuleTextFile(string path, byte[] buffer) : base(path)
        {
            Content = Encoding.UTF8.GetString(buffer);
        }
    }
}
