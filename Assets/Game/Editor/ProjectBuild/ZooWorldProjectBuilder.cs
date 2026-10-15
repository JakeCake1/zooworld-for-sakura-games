using UnityEditor;
using UnityEngine;
using ZooWorld.Editor.AnimalScaffolding;
using ZooWorld.Game.Features.Animals.Common.Data;

namespace ZooWorld.Editor.ProjectBuild
{
    public sealed class ZooWorldProjectBuilder
    {
        private readonly EditorFolderUtility _folders = new EditorFolderUtility();
        private readonly MaterialAssetBuilder _materials = new MaterialAssetBuilder();
        private readonly AnimalContentBuilder _animals = new AnimalContentBuilder();
        private readonly SceneContentBuilder _scene = new SceneContentBuilder();

        [MenuItem("Zoo World/Rebuild Demo Content")]
        public static void Build()
        {
            new ZooWorldProjectBuilder().BuildProject();
        }

        private void BuildProject()
        {
            EnsureContentFolders();
            var groundMaterial = BuildGroundMaterial();
            _animals.BuildFrog();
            _animals.BuildSnake();
            ReloadAssets();
            _scene.Build(groundMaterial);
            ConfigureProject();
            SaveProject();
            Debug.Log("Zoo World demo content rebuilt successfully.");
        }

        private void EnsureContentFolders()
        {
            _folders.Ensure(ZooWorldProjectPaths.FrogContent);
            _folders.Ensure(ZooWorldProjectPaths.SnakeContent);
            _folders.Ensure(ZooWorldProjectPaths.EnvironmentContent);
            _folders.Ensure(ZooWorldProjectPaths.Scenes);
        }

        private Material BuildGroundMaterial()
        {
            return _materials.Create(
                ZooWorldProjectPaths.EnvironmentContent + "/Ground.mat",
                new Color(0.36f, 0.20f, 0.08f));
        }

        private void ReloadAssets()
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private void ConfigureProject()
        {
            PlayerSettings.productName = "Zoo World";
            EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(ZooWorldProjectPaths.MainScene, true) };
        }

        private void SaveProject()
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
