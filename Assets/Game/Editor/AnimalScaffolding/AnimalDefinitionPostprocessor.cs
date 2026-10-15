using UnityEditor;

namespace ZooWorld.Editor.AnimalScaffolding
{
    internal sealed class AnimalDefinitionPostprocessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(
            string[] importedAssets,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            RegisterAll(importedAssets);
            RegisterAll(movedAssets);
        }

        private static void RegisterAll(string[] assetPaths)
        {
            for (var index = 0; index < assetPaths.Length; index++)
            {
                AnimalDefinitionAddressables.Register(assetPaths[index]);
            }
        }
    }
}
