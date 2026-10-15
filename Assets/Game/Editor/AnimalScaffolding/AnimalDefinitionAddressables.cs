using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using ZooWorld.Game.Features.Animals.Common.Data;
using ZooWorld.Services.Configuration;

namespace ZooWorld.Editor.AnimalScaffolding
{
    internal static class AnimalDefinitionAddressables
    {
        public static void Register(string assetPath)
        {
            if (AssetDatabase.LoadAssetAtPath<AnimalDefinition>(assetPath) == null)
            {
                return;
            }

            RegisterDefinition(assetPath);
        }

        private static void RegisterDefinition(string assetPath)
        {
            var settings = AddressableAssetSettingsDefaultObject.GetSettings(true);
            settings.AddLabel(ScriptableObjectAnimalConfigSource.AddressablesLabel);
            var entry = CreateEntry(settings, assetPath);
            entry.SetLabel(ScriptableObjectAnimalConfigSource.AddressablesLabel, true, true);
            settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryModified, entry, true);
        }

        private static AddressableAssetEntry CreateEntry(AddressableAssetSettings settings, string assetPath)
        {
            var guid = AssetDatabase.AssetPathToGUID(assetPath);
            var entry = settings.CreateOrMoveEntry(guid, settings.DefaultGroup);
            entry.address = $"animal-definitions/{guid}";
            return entry;
        }
    }
}
