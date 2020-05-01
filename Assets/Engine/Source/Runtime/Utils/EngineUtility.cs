using TinyJSON;
using UnityEngine;

namespace KAG.Runtime.Utils
{
    public class EngineUtility : BaseUtility
    {
        public float deltaTime { get { return Time.deltaTime; } }
        public float fixedDeltaTime { get { return Time.fixedDeltaTime; } }

        public EngineUtility(GameEngine engine) : base(engine)
        {
            engine.SetObject("Engine", this);
        }

        public void Include(string filePath)
        {
            engine.ExecuteFile(filePath);
        }

        public object FromJson(string filePath)
        {
            JsonFile file = engine.Get<JsonFile>(filePath);
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
