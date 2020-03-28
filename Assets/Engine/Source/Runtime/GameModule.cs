using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Jint;
using Jint.Native;
using Jint.Runtime;
using Jint.Runtime.Interop;

namespace KAG.Runtime
{
    public class GameModule
    {
        public Engine jint;
        public List<BaseUtils> utils = new List<BaseUtils>();
        public Dictionary<string, GameModuleFile> files = new Dictionary<string, GameModuleFile>();

        public GameModule(Stream zipStream)
        {
            jint = new Engine();
            ZipArchive archive = new ZipArchive(zipStream);

            foreach (var entry in archive.Entries)
            {
                Add(entry.FullName, entry.Open());
            }

            utils.Add(new GameUtils(this));
            utils.Add(new GameDebug(this));
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
                    file = new GameModuleScriptFile(fileBuffer);
                    break;
                case ".txt":
                    file = new GameModuleTextFile(fileBuffer);
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
    }

    public class GameModuleFile { }

    public class GameModuleTextFile : GameModuleFile
    {
        public string Content { get; } = "";

        public GameModuleTextFile(byte[] buffer)
        {
            Content = Encoding.UTF8.GetString(buffer);
        }
    }

    public class GameModuleScriptFile : GameModuleTextFile
    {
        public GameModuleScriptFile(byte[] buffer) : base(buffer) { }

        /// <summary>
        /// Execute this script with the specified engine
        /// </summary>
        /// <param name="jint">The JavaScript engine to use for executing the script</param>
        public void Run(Engine jint)
        {
            jint.Execute(Content);
        }
    }
}
