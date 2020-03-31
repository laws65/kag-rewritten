using System.IO;
using UnityEngine;

namespace KAG.Runtime
{
    using KAG.Misc;

    public class GameRuntime : Singleton<GameRuntime>
    {
        private GameModule module;

        private void Awake()
        {
            LoadBase();
        }

        private void Start()
        {
            module.Run("Scripts/Main.js");
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
