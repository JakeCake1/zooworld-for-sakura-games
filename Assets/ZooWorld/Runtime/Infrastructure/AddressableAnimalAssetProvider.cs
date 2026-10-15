using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace ZooWorld.Infrastructure
{
    public sealed class AddressableAnimalAssetProvider : IAnimalAssetProvider
    {
        public void InstantiateAsync(
            string address,
            Vector3 position,
            Transform parent,
            Action<GameObject> onLoaded)
        {
            var operation = Addressables.InstantiateAsync(address, position, Quaternion.identity, parent);
            operation.Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    onLoaded?.Invoke(handle.Result);
                    return;
                }

                Debug.LogError($"Failed to load addressable animal prefab '{address}'.");
                Addressables.Release(handle);
            };
        }

        public void Release(GameObject instance)
        {
            if (instance != null)
            {
                Addressables.ReleaseInstance(instance);
            }
        }
    }
}
