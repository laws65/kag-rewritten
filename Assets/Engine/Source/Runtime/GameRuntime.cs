using System.IO;
using Jint;
using UnityEngine;

namespace KAG.Runtime
{
    using KAG.Misc;
    using KAG.Runtime.Utils;
    using KAG.Runtime.Types;
    using KAG.Runtime.Modules;

    public class GameRuntime : Singleton<GameRuntime>
    {
        public Engine Engine { get => module.engine; }
        GameModule module;

        private void Awake()
        {
            LoadBase();
        }

        private void Start()
        {
            module.Execute("Main.js");
            module.ExecuteString("Main.Start()");
        }

        private void LoadBase()
        {
            TextAsset zipBinary = Resources.Load(GamePackager.BASE_PACKAGE) as TextAsset;
            Stream zipStream = new MemoryStream(zipBinary.bytes);

            module = new GameModule(zipStream);
        }

        private void LoadModule(string url)
        {

        }

        public T Get<T>(string path)
        {
            return module.Get<T>(path);
        }
    }
}
