#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
#endif

using System;
using System.IO;
using System.IO.Compression;
using UnityEngine;
using ICSharpCode.SharpZipLib.Zip;

namespace KAG
{
    public static class GamePackager
    {
        public static string BASE_PACKAGE = "Base";
        public static string BASE_DIRECTORY = Application.dataPath + Path.DirectorySeparatorChar + BASE_PACKAGE;
        public static string CACHE_DIRECTORY = Application.dataPath + Path.DirectorySeparatorChar + "__LOCAL__" + Path.DirectorySeparatorChar + "Resources";

        public static void Pack(string input_dir, string output_dir, Action callback = null)
        {
            string package_name = Path.GetFileName(input_dir) + ".bytes";
            string package_path = output_dir + "/" + package_name;

            if (File.Exists(package_path))
            {
                File.Delete(package_path);
            }

            Directory.CreateDirectory(output_dir);
            using (var zip = ZipFile.Create(package_path))
            {
                zip.BeginUpdate();
                foreach (var path in Directory.EnumerateFiles(input_dir, "*.*", SearchOption.AllDirectories))
                {
                    if (Path.GetExtension(path) != ".meta")
                    {
                        zip.Add(path, path.TrimStart(input_dir.ToCharArray()));
                    }
                }
                zip.CommitUpdate();
            }

            callback?.Invoke();
        }
    }

#if UNITY_EDITOR
    [InitializeOnLoad]
    public static class GamePackager_Editor
    {
        static bool requiresUpdate = true;

        static GamePackager_Editor()
        {
            EditorApplication.playModeStateChanged += OnPlay;
        }

        static void OnPlay(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingEditMode && requiresUpdate)
            {
                requiresUpdate = false;

                EditorApplication.isPlaying = false;
                GamePackager.Pack(GamePackager.BASE_DIRECTORY, GamePackager.CACHE_DIRECTORY, () =>
                {
                    AssetDatabase.Refresh();
                    EditorApplication.isPlaying = true;
                });
            }

            if (state == PlayModeStateChange.EnteredPlayMode)
            {
                requiresUpdate = true;
            }
        }
    }

    public class GamePackager_Standalone : IPreprocessBuildWithReport, IPostprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            GamePackager.Pack(GamePackager.BASE_DIRECTORY, GamePackager.CACHE_DIRECTORY);
        }

        public void OnPostprocessBuild(BuildReport report)
        {

        }
    }
#endif
}
