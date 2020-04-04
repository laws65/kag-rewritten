using System.IO;
using UnityEngine;

namespace KAG.Runtime
{
    using KAG.Misc;
    using KAG.Runtime.Types;
    using KAG.Runtime.Utils;

    public class GameRuntime : Singleton<GameRuntime>
    {
        private GameModule module;

        private void Awake()
        {
            LoadBase();

            module.SetGlobalType("Tile", typeof(KTile));

            new GameUtils(module);
            new EngineUtils(module);
            new MapUtils(module);
            new DebugUtils(module);
            new AssertUtils(module);
        }

        private void Start()
        {
            module.Execute("Scripts/Main.js");
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
    }
}
