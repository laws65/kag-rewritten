using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using Jint;
using Jint.Native;
using Jint.Native.Json;
using Jint.Runtime;
using Jint.Runtime.Interop;

namespace KAG.Runtime
{
    using ICSharpCode.SharpZipLib.Zip;
    using KAG.Runtime.Utils;

    public class GameModule
    {
        public Engine jint;
        public Dictionary<string, GameModuleFile> files = new Dictionary<string, GameModuleFile>();

        #region Cached frequently used helpers
        public JsonSerializer jsonSerializer;
        public JsonParser jsonParser;
        #endregion

        public GameModule(Stream zipStream)
        {
            jint = new Engine();

            ZipFile archive = new ZipFile(zipStream);
            foreach (ZipEntry entry in archive)
            {
                Add(entry.Name, archive.GetInputStream(entry));
            }

            jsonSerializer = new JsonSerializer(jint);
            jsonParser = new JsonParser(jint);

            new GameUtils(this);
            new DebugUtils(this);
            new AssertUtils(this);
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
            switch (fileExtension)
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
        public void Run(string filePath)
        {
            foreach (var pair in files)
            {
                if (pair.Key == filePath && pair.Value is GameModuleScriptFile)
                {
                    var script = pair.Value as GameModuleScriptFile;
                    script.Run(jint);
                }
            }
        }

        /// <summary>
        /// Bind a .NET object in the JavaScript runtime
        /// </summary>
        /// <param name="name">The name that will be used when referring to it in JavaScript</param>
        /// <param name="obj">The object to bind</param>
        public void SetGlobalObject(string name, object obj)
        {
            jint.SetValue(name, JsValue.FromObject(jint, obj));
        }

        /// <summary>
        /// Bind a .NET type in the JavaScript runtime
        /// </summary>
        /// <param name="name">The name that will be used when referring to it in JavaScript</param>
        /// <param name="obj">The type to bind</param>
        public void SetGlobalType(string name, Type type)
        {
            jint.SetValue(name, TypeReference.CreateTypeReference(jint, type));
        }

        #region Helpers
        public JsValue FromJson(string json)
        {
            return jsonParser.Parse(json);
        }

        public JsValue ToJson(JsValue value)
        {
            if (value is JsString)
            {
                return value;
            }

            return jsonSerializer.Serialize(value, JsValue.Null, JsValue.Null);
        }
        #endregion
    }

    public class GameModuleFile
    {
        public GameModule module;

        public GameModuleFile(GameModule gameModule)
        {
            module = gameModule;
        }
    }

    public class GameModuleTextFile : GameModuleFile
    {
        public string Text { get; } = "";

        public GameModuleTextFile(GameModule gameModule, byte[] buffer) : base(gameModule)
        {
            Text = Encoding.UTF8.GetString(buffer);
        }
    }

    public class GameModuleJsonFile : GameModuleTextFile
    {
        public JsValue Value
        {
            get
            {
                return module.FromJson(Text);
            }
        }

        public GameModuleJsonFile(GameModule gameModule, byte[] buffer) : base(gameModule, buffer) { }
    }

    public class GameModuleScriptFile : GameModuleTextFile
    {
        public GameModuleScriptFile(GameModule gameModule, byte[] buffer) : base(gameModule, buffer) { }

        /// <summary>
        /// Execute this script with the specified engine
        /// </summary>
        /// <param name="jint">The JavaScript engine to use for executing the script</param>
        public void Run(Engine jint)
        {
            jint.Execute(Text);
        }
    }
}
