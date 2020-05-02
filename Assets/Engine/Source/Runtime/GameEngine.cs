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

        /// <summary>
        /// Add a file to this module
        /// </summary>
        /// <param name="filePath">The path that will be used when retrieving this file</param>
        /// <param name="fileStream">The file's binary data</param>
        /// <param name="fileExtension">If left null, will use the filePath extension to decide the type of data the file represents</param>
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

        /// <summary>
        /// Remove a file from this module
        /// </summary>
        /// <param name="filePath">The path where the file is located</param>
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

        /// <summary>
        /// Execute a script from a specified file
        /// </summary>
        /// <param name="filePath">The path to the script file</param>
        public Engine ExecuteFile(string filePath)
        {
            var script = Get<ScriptFile>(filePath);
            return Execute(script.Text);
        }

        /// <summary>
        /// Bind a .NET object in the JavaScript runtime
        /// </summary>
        /// <param name="name">The name that will be used when referring to it in JavaScript</param>
        /// <param name="obj">The object to bind</param>
        public void SetObject(string name, object obj)
        {
            SetValue(name, JsValue.FromObject(this, obj));
        }

        /// <summary>
        /// Bind a .NET type in the JavaScript runtime
        /// </summary>
        /// <param name="name">The name that will be used when referring to it in JavaScript</param>
        /// <param name="obj">The type to bind</param>
        public void SetType(string name, Type type)
        {
            SetValue(name, TypeReference.CreateTypeReference(this, type));
        }

        public JsValue FromClass(string className)
        {
            return Execute($"new {className}()").GetCompletionValue();
        }
    }
}
