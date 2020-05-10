using System;
using System.IO;
using System.Collections.Generic;
using ICSharpCode.SharpZipLib.Zip;
using Jint;
using Jint.Native;
using Jint.Native.Json;
using Jint.Runtime;
using Jint.Runtime.Interop;
using UnityEngine;

namespace KAG.Runtime
{
    using KAG.Runtime.Types;
    using KAG.Runtime.Utils;

    public class GameEngine : Engine
    {
        public Dictionary<string, File> files;

        #region Cached helpers
        public JsonParser jsonParser;
        #endregion

        #region Initializing
        public GameEngine() : base()
        {
            jsonParser = new JsonParser(this);
            files = new Dictionary<string, File>();

            LoadBase();
            SetType("Tile", typeof(KTile));
            SetType("Sprite", typeof(KSprite));
            SetType("Texture", typeof(KTexture));
            SetType("Vector2", typeof(KVector2));
            SetType("Vector3", typeof(KVector3));
            SetType("Vector2Int", typeof(KVector2Int));
            SetType("Vector3Int", typeof(KVector3Int));
            SetType("Color", typeof(KColor));

            new AssertUtility(this);
            new DebugUtility(this);
            new EngineUtility(this);
            new MapUtility(this);
        }

        private void LoadBase()
        {
            TextAsset zipBinary = Resources.Load(GamePackager.BASE_PACKAGE) as TextAsset;
            Stream zipStream = new MemoryStream(zipBinary.bytes);

            ZipFile archive = new ZipFile(zipStream);
            foreach (ZipEntry entry in archive)
            {
                Add(entry.Name, archive.GetInputStream(entry));
            }
        }

        private void LoadModule(string url)
        {

        }
        #endregion

        #region File management
        public void Add(string filePath, Stream fileStream, string fileExtension = null)
        {
            filePath = filePath.Replace("\\", "/");
            fileExtension = fileExtension ?? Path.GetExtension(filePath);

            byte[] fileBuffer;
            using (var ms = new MemoryStream())
            {
                fileStream.CopyTo(ms);
                fileBuffer = ms.ToArray();
            }

            File file = null;
            switch (fileExtension.ToLower())
            {
                case ".js":
                    file = new ScriptFile(this, fileBuffer);
                    break;
                case ".json":
                    file = new JsonFile(this, fileBuffer);
                    break;
                case ".txt":
                    file = new TextFile(this, fileBuffer);
                    break;
                case ".jpg":
                case ".jpeg":
                case ".png":
                    file = new TextureFile(this, fileBuffer);
                    break;
            }

            if (file != null)
            {
                files.Add(filePath, file);
            }
        }
        
        public void Remove(string filePath)
        {
            files.Remove(filePath);
        }

        public File Get<File>(string filePath)
        {
            if (files.ContainsKey("Build/" + filePath))
            {
                return (File)(object)files["Build/" + filePath];
            }
            else if (files.ContainsKey(filePath))
            {
                return (File)(object)files[filePath];
            }

            return default;
        }
        #endregion

        #region Runtime utilities
        public JsValue Import(string scriptPath)
        {
            var file = Get<ScriptFile>(scriptPath);
            if (file.imported == false)
            {
                file.importedValue = ExecuteFile(scriptPath).GetCompletionValue();
            }
            return file.importedValue;
        }

        public Engine ExecuteFile(string scriptPath)
        {
            var script = Get<ScriptFile>(scriptPath);
            return ExecuteScript(script.Text);
        }

        public Engine ExecuteScript(string scriptText)
        {
            return Execute(scriptText);
        }

        public void SetObject(string name, object obj)
        {
            SetValue(name, JsValue.FromObject(this, obj));
        }

        public void SetType(string name, Type type)
        {
            SetValue(name, TypeReference.CreateTypeReference(this, type));
        }

        public JsValue FromClass(string className)
        {
            return FromClass(Global.Get(className));
        }

        public JsValue FromClass(JsValue classFunction)
        {
            return Invoke(Global.Get("Reflect").Get("construct"), JsValue.Undefined, new object[] { classFunction, Execute("[]").GetCompletionValue().ToObject() });
        }
        #endregion
    }
}
