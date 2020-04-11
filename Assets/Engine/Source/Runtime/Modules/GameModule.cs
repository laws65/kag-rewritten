using System;
using System.IO;
using System.Collections.Generic;
using ICSharpCode.SharpZipLib.Zip;
using Jint;
using Jint.Native;
using Jint.Native.Json;
using Jint.Runtime;
using Jint.Runtime.Interop;

namespace KAG.Runtime.Modules
{
    using KAG.Runtime.Utils;

    public class GameModule
    {
        #region Cached frequently used helpers
        public JsonParser jsonParser;
        #endregion

        public Engine engine;
        public Dictionary<string, GameModuleFile> files = new Dictionary<string, GameModuleFile>();

        public GameModule(Stream zipStream)
        {
            engine = new Engine();
            jsonParser = new JsonParser(engine);

            ZipFile archive = new ZipFile(zipStream);
            foreach (ZipEntry entry in archive)
            {
                Add(entry.Name, archive.GetInputStream(entry));
            }
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

            GameModuleFile file = null;
            switch (fileExtension.ToLower())
            {
                case ".js":
                    file = new GameModuleScriptFile(this, fileBuffer);
                    break;
                case ".json":
                    file = new GameModuleJsonFile(this, fileBuffer);
                    break;
                case ".txt":
                    file = new GameModuleTextFile(this, fileBuffer);
                    break;
                case ".jpg":
                case ".jpeg":
                case ".png":
                    file = new GameModuleTextureFile(this, fileBuffer);
                    break;
            }

            if (file is GameModuleFile)
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

        public T Get<T>(string filePath)
        {
            if (files.ContainsKey(filePath))
            {
                return (T)(object)files[filePath];
            }

            return default;
        }

        /// <summary>
        /// Execute a script from a specified file
        /// </summary>
        /// <param name="filePath">The path to the script file</param>
        public void Execute(string filePath)
        {
            var script = Get<GameModuleScriptFile>(filePath);
            script?.Run();
        }

        public void ExecuteString(string script)
        {
            engine.Execute(script);
        }

        /// <summary>
        /// Bind a .NET object in the JavaScript runtime
        /// </summary>
        /// <param name="name">The name that will be used when referring to it in JavaScript</param>
        /// <param name="obj">The object to bind</param>
        public void SetGlobalObject(string name, object obj)
        {
            engine.SetValue(name, JsValue.FromObject(engine, obj));
        }

        /// <summary>
        /// Bind a .NET type in the JavaScript runtime
        /// </summary>
        /// <param name="name">The name that will be used when referring to it in JavaScript</param>
        /// <param name="obj">The type to bind</param>
        public void SetGlobalType(string name, Type type)
        {
            engine.SetValue(name, TypeReference.CreateTypeReference(engine, type));
        }
    }
}
