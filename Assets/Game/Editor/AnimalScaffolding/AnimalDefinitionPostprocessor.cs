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
            var addressables = new AnimalDefinitionAddressables();
            RegisterAll(importedAssets, addressables);
            RegisterAll(movedAssets, addressables);
        }

        private static void RegisterAll(string[] assetPaths, AnimalDefinitionAddressables addressables)
        {
            for (var index = 0; index < assetPaths.Length; index++)
            {
                addressables.Register(assetPaths[index]);
            }
        }
    }
}
