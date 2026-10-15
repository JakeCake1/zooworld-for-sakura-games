using UnityEditor;
using UnityEngine;
using ZooWorld.Editor.AnimalScaffolding;
using ZooWorld.Game.Features.Animals.Common.Data;

namespace ZooWorld.Editor.ProjectBuild
{
    public static class ZooWorldProjectBuilder
    {
        [MenuItem("Zoo World/Rebuild Demo Content")]
        public static void Build()
        {
            EnsureContentFolders();
            var groundMaterial = BuildGroundMaterial();
            AnimalContentBuilder.BuildFrog();
            AnimalContentBuilder.BuildSnake();
            ReloadAssets();
            SceneContentBuilder.Build(groundMaterial);
            ConfigureProject();
            SaveProject();
            Debug.Log("Zoo World demo content rebuilt successfully.");
        }

        private static void EnsureContentFolders()
        {
            EditorFolderUtility.Ensure(ZooWorldProjectPaths.FrogContent);
            EditorFolderUtility.Ensure(ZooWorldProjectPaths.SnakeContent);
            EditorFolderUtility.Ensure(ZooWorldProjectPaths.EnvironmentContent);
            EditorFolderUtility.Ensure(ZooWorldProjectPaths.Scenes);
        }

        private static Material BuildGroundMaterial()
        {
            return MaterialAssetBuilder.Create(
                ZooWorldProjectPaths.EnvironmentContent + "/Ground.mat",
                new Color(0.36f, 0.20f, 0.08f));
        }

        private static void ReloadAssets()
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void ConfigureProject()
        {
            PlayerSettings.productName = "Zoo World";
            EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(ZooWorldProjectPaths.MainScene, true) };
        }

        private static void SaveProject()
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
