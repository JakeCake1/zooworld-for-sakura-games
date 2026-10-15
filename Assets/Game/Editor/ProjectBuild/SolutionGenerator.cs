using System;
using System.IO;
using System.Linq;
using Unity.CodeEditor;
using UnityEditor;
using UnityEngine;

namespace ZooWorld.Editor.ProjectBuild
{
    public sealed class SolutionGenerator
    {
        [MenuItem("Zoo World/Generate Rider Solution")]
        public static void Generate()
        {
            new SolutionGenerator().GenerateSolution();
        }

        private void GenerateSolution()
        {
            AssetDatabase.Refresh();
            SelectRiderEditor();
            CodeEditor.CurrentEditor.SyncAll();
            LogSolutionPath();
        }

        private void SelectRiderEditor()
        {
            CodeEditor.SetExternalScriptEditor(FindRiderExecutable());
        }

        private string FindRiderExecutable()
        {
            var jetBrainsDirectory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                "JetBrains");

            return Directory
                .EnumerateFiles(jetBrainsDirectory, "rider64.exe", SearchOption.AllDirectories)
                .OrderByDescending(File.GetLastWriteTimeUtc)
                .First();
        }

        private void LogSolutionPath()
        {
            var projectRoot = Directory.GetParent(Application.dataPath)?.FullName;
            Debug.Log($"Rider solution generated in: {projectRoot}");
        }
    }
}
