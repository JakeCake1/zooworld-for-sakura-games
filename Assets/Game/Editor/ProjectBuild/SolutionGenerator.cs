using System;
using System.IO;
using System.Linq;
using Unity.CodeEditor;
using UnityEditor;
using UnityEngine;

namespace ZooWorld.Editor.ProjectBuild
{
    public static class SolutionGenerator
    {
        [MenuItem("Zoo World/Generate Rider Solution")]
        public static void Generate()
        {
            AssetDatabase.Refresh();
            SelectRiderEditor();
            EnsureSynchronized();
            LogSolutionPath();
        }

        private static void SelectRiderEditor()
        {
            CodeEditor.SetExternalScriptEditor(FindRiderExecutable());
        }

        private static string FindRiderExecutable()
        {
            var jetBrainsDirectory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                "JetBrains");

            return Directory
                .EnumerateFiles(jetBrainsDirectory, "rider64.exe", SearchOption.AllDirectories)
                .OrderByDescending(File.GetLastWriteTimeUtc)
                .First();
        }

        private static void EnsureSynchronized()
        {
            CodeEditor.CurrentEditor.SyncAll();
        }

        private static void LogSolutionPath()
        {
            var projectRoot = Directory.GetParent(Application.dataPath)?.FullName;
            Debug.Log($"Rider solution generated in: {projectRoot}");
        }
    }
}
