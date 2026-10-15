using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using ZooWorld.Game.Features.Animals.Common.Data;
using ZooWorld.Services.Configuration;

namespace ZooWorld.Editor.AnimalScaffolding
{
    internal sealed class AnimalDefinitionAddressables
    {
        public void Register(string assetPath)
        {
            if (AssetDatabase.LoadAssetAtPath<AnimalDefinition>(assetPath) == null)
            {
                return;
            }

            RegisterDefinition(assetPath);
        }

        public void RegisterPrefab(string assetPath, string address)
        {
            var settings = AddressableAssetSettingsDefaultObject.GetSettings(true);
            var entry = CreateEntry(settings, assetPath);
            entry.address = address;
            MarkDirty(settings, entry);
        }

        private void RegisterDefinition(string assetPath)
        {
            var settings = AddressableAssetSettingsDefaultObject.GetSettings(true);
            settings.AddLabel(ScriptableObjectAnimalConfigSource.AddressablesLabel);
            var entry = CreateEntry(settings, assetPath);
            entry.SetLabel(ScriptableObjectAnimalConfigSource.AddressablesLabel, true, true);
            MarkDirty(settings, entry);
        }

        private AddressableAssetEntry CreateEntry(AddressableAssetSettings settings, string assetPath)
        {
            var guid = AssetDatabase.AssetPathToGUID(assetPath);
            var entry = settings.CreateOrMoveEntry(guid, settings.DefaultGroup);
            entry.address = $"animal-definitions/{guid}";
            return entry;
        }

        private void MarkDirty(AddressableAssetSettings settings, AddressableAssetEntry entry)
        {
            settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryModified, entry, true);
        }
    }
}
