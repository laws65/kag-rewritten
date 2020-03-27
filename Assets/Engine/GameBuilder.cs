#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using System.IO.Compression;
using System.IO;

namespace KAG
{
    [InitializeOnLoad]
    public static class ProjectBuild
    {
        public static void Pack(string input_dir, string output_dir)
        {
            string package_name = new DirectoryInfo(input_dir).Name + ".bytes";
            string package_path = output_dir + "/" + package_name;

            if (File.Exists(package_path))
            {
                File.Delete(package_path);
            }

            ZipFile.CreateFromDirectory(input_dir, package_path);
        }

        static ProjectBuild()
        {
            EditorApplication.playModeStateChanged += OnPlay;
        }

        static void OnPlay(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredPlayMode)
            {
                ProjectBuild.Pack(Application.dataPath + "/Base", Application.dataPath + "/__LOCAL__/Resources");
            }
        }
    }

    public class ProjectBuildEvent : IPreprocessBuildWithReport, IPostprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            ProjectBuild.Pack(Application.dataPath + "/Base", Application.dataPath + "/__LOCAL__/Resources");
        }

        public void OnPostprocessBuild(BuildReport report)
        {

        }
    }

#endif
}
