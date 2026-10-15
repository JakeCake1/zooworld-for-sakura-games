using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using ZooWorld.Game.Features.Spawning;

namespace ZooWorld.Services.Assets
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
            operation.Completed += handle => CompleteInstantiation(handle, address, onLoaded);
        }

        public void Release(GameObject instance)
        {
            if (instance != null)
            {
                Addressables.ReleaseInstance(instance);
            }
        }

        private static void CompleteInstantiation(
            AsyncOperationHandle<GameObject> handle,
            string address,
            Action<GameObject> onLoaded)
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                onLoaded?.Invoke(handle.Result);
                return;
            }

            ReportFailure(handle, address);
        }

        private static void ReportFailure(AsyncOperationHandle<GameObject> handle, string address)
        {
            Debug.LogError($"Failed to load addressable animal prefab '{address}'.");
            Addressables.Release(handle);
        }
    }
}
