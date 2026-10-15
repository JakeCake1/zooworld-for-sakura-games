using System;
using UnityEditor;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace ZooWorld.Editor.ProjectBuild
{
    public static class VerificationPlayerBuilder
    {
        [MenuItem("Zoo World/Build Windows Verification Player")]
        public static void Build()
        {
            ZooWorldProjectBuilder.Build();
            BuildAddressables();
            var report = BuildPlayer();
            EnsureSucceeded(report);
            Debug.Log($"Verification player built: {report.summary.totalSize} bytes.");
        }

        private static void BuildAddressables()
        {
            AddressableAssetSettings.BuildPlayerContent(out AddressablesPlayerBuildResult result);
            if (!string.IsNullOrEmpty(result.Error))
            {
                throw new InvalidOperationException(result.Error);
            }
        }

        private static BuildReport BuildPlayer()
        {
            return BuildPipeline.BuildPlayer(new BuildPlayerOptions
            {
                scenes = new[] { ZooWorldProjectPaths.MainScene },
                locationPathName = "Build/Windows/ZooWorld.exe",
                target = BuildTarget.StandaloneWindows64,
                options = BuildOptions.Development
            });
        }

        private static void EnsureSucceeded(BuildReport report)
        {
            if (report.summary.result != BuildResult.Succeeded)
            {
                throw new InvalidOperationException($"Player build failed: {report.summary.result}, {report.summary.totalErrors} error(s).");
            }
        }
    }
}
