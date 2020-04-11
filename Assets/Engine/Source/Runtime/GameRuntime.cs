using System.IO;
using UnityEngine;

namespace KAG.Runtime
{
    using KAG.Misc;
    using KAG.Runtime.Utils;
    using KAG.Runtime.Types;
    using KAG.Runtime.Modules;

    public class GameRuntime : Singleton<GameRuntime>
    {
        public GameModule module;

        private void Awake()
        {
            LoadBase();

            module.SetGlobalType("Texture", typeof(KTexture));
            module.SetGlobalType("Sprite", typeof(KSprite));
            module.SetGlobalType("Tile", typeof(KTile));
            module.SetGlobalType("Color", typeof(KColor));
            module.SetGlobalType("Vector2", typeof(KVector2));
            module.SetGlobalType("Vector3", typeof(KVector3));

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
