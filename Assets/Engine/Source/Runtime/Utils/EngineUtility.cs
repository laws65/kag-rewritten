using System.Collections.Generic;
using TinyJSON;
using Jint;
using Jint.Native;
using UnityEngine;

namespace KAG.Runtime.Utils
{
    public class EngineUtility : Utility
    {
        private Dictionary<string, JsValue> includes = new Dictionary<string, JsValue>();

        public float deltaTime { get { return Time.deltaTime; } }
        public float fixedDeltaTime { get { return Time.fixedDeltaTime; } }

        public EngineUtility(GameEngine engine) : base(engine)
        {
            engine.SetObject("Engine", this);
        }

        public JsValue Import(string filePath)
        {
            if (!includes.ContainsKey(filePath))
            {
                includes[filePath] = engine.ExecuteFile(filePath).GetCompletionValue();
            }

            return includes[filePath];
        }

        public JsValue ImportUnsafe(string filePath)
        {
            return engine.ExecuteFile(filePath).GetCompletionValue();
        }

        public JsValue Export(JsValue obj)
        {
            return obj;
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
