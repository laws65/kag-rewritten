using System.IO;
using System.IO.Compression;
using UnityEngine;

namespace KAG
{
    public class GameRuntime : Singleton<GameRuntime>
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            LoadBase();
        }

        public void LoadBase()
        {
            TextAsset zipBinary = Resources.Load(GamePackager.BASE_PACKAGE) as TextAsset;
            Stream zipStream = new MemoryStream(zipBinary.bytes);

            ZipArchive zipArchive = new ZipArchive(zipStream);
            foreach (var entry in zipArchive.Entries)
            {
                Debug.Log(entry.FullName);
            }
        }

        public void LoadMod(string mod_name)
        {

        }
    }
}
