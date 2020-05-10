using System.Collections.Generic;
using TinyJSON;
using Jint;
using Jint.Native;
using UnityEngine;
using Mirror;

namespace KAG.Runtime.Utils
{
    public class EngineUtility : Utility
    {
        public float deltaTime { get { return Time.deltaTime; } }
        public float fixedDeltaTime { get { return Time.fixedDeltaTime; } }

        public void Instantiate(string classPath)
        {
            GameEntity entity = Object.Instantiate(GameManager.Instance.entityPrefab).GetComponent<GameEntity>();
            entity.Init(classPath);

            NetworkServer.Spawn(entity.gameObject);
        }

        public EngineUtility(GameEngine engine) : base(engine)
        {
            engine.SetObject("Engine", this);
        }

        public JsValue Import(string scriptPath)
        {
            return engine.Import(scriptPath);
        }

        public JsValue ImportUnsafe(string scriptPath)
        {
            return engine.Import(scriptPath);
        }

        public JsValue Export(JsValue obj)
        {
            return obj;
        }

        public JsValue FromClass(JsValue classFunction)
        {
            return engine.FromClass(classFunction);
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
