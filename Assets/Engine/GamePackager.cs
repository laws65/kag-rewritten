#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
#endif

using UnityEngine;
using System.IO;
using System.IO.Compression;

namespace KAG
{
    public static class GamePackager
    {
        public static string BASE_PACKAGE = "Base";
        public static string BASE_DIRECTORY = Application.dataPath + Path.DirectorySeparatorChar + BASE_PACKAGE;
        public static string CACHE_DIRECTORY = Application.dataPath + Path.DirectorySeparatorChar + "__LOCAL__" + Path.DirectorySeparatorChar + "Resources";
    }

#if UNITY_EDITOR
    [InitializeOnLoad]
    public static class GamePackager_Editor
    {
        public static void Pack(string input_dir, string output_dir)
        {
            string package_name = new DirectoryInfo(input_dir).Name + ".bytes";
            string package_path = output_dir + "/" + package_name;

            if (File.Exists(package_path))
            {
                File.Delete(package_path);
            }

            var zip = ZipFile.Open(package_path, ZipArchiveMode.Create);
            foreach (var file in Directory.EnumerateFiles(input_dir, "*.*", SearchOption.AllDirectories))
            {
                if (Path.GetExtension(file).EndsWith(".meta"))
                {
                    continue;
                }
                zip.CreateEntry(file.TrimStart(input_dir.ToCharArray()));
            }
            zip.Dispose();
        }

        static GamePackager_Editor()
        {
            EditorApplication.playModeStateChanged += OnPlay;
        }

        static void OnPlay(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredPlayMode)
            {
                GamePackager_Editor.Pack(GamePackager.BASE_DIRECTORY, GamePackager.CACHE_DIRECTORY);
            }
        }
    }

    public class GamePackager_Standalone : IPreprocessBuildWithReport, IPostprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            GamePackager_Editor.Pack(GamePackager.BASE_DIRECTORY, GamePackager.CACHE_DIRECTORY);
        }

        public void OnPostprocessBuild(BuildReport report)
        {

        }
    }
#endif
}
