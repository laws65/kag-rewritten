using System.IO;
using UnityEngine;

namespace KAG
{
    public class GameRuntime : Singleton<GameRuntime>
    {
        public GameModule baseModule;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            LoadBase();

            // Giving it a test
            foreach (var file in baseModule.fileList)
            {
                if (file is GameModuleTextFile)
                {
                    Debug.Log(((GameModuleTextFile)file).Content);
                }
            }
        }

        public void LoadBase()
        {
            TextAsset zipBinary = Resources.Load(GamePackager.BASE_PACKAGE) as TextAsset;
            Stream zipStream = new MemoryStream(zipBinary.bytes);

            baseModule = new GameModule(zipStream);
        }

        public void LoadModule(string url)
        {

        }
    }
}
