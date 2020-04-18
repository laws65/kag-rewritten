using TinyJSON;
using UnityEngine;

namespace KAG.Runtime.Utils
{
    using KAG.Runtime.Modules;

    public class EngineUtils : BaseUtils
    {
        public float deltaTime { get { return Time.deltaTime; } }
        public float fixedDeltaTime { get { return Time.fixedDeltaTime; } }

        public EngineUtils(GameModule gameModule) : base(gameModule)
        {
            module.SetObject("Engine", this);
        }

        public void Include(string filePath)
        {
            module.Execute(filePath);
        }

        public object FromJson(string filePath)
        {
            JsonFile file = module.Get<JsonFile>(filePath);
            return file?.Value;
        }

        public string ToJson(object value, bool prettify = false)
        {
            EncodeOptions options = EncodeOptions.NoTypeHints;
            if (prettify)
            {
                options |= EncodeOptions.PrettyPrint;
            }

            return JSON.Dump(value, options);
        }
    }
}
