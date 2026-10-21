using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using ZooWorld.Game.Features.Animals.Common.Presentation;

namespace ZooWorld.Game.Features.Spawning
{
    public sealed class AnimalPool : IDisposable
    {
        private readonly IAnimalAssetProvider _assetProvider;
        private readonly Dictionary<string, Stack<GameObject>> _available =
            new Dictionary<string, Stack<GameObject>>();
        private readonly Dictionary<int, string> _addresses = new Dictionary<int, string>();
        private readonly HashSet<int> _availableIds = new HashSet<int>();
        private readonly HashSet<GameObject> _owned = new HashSet<GameObject>();

        public AnimalPool(IAnimalAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        public async UniTask<GameObject> RentAsync(
            string address,
            Vector3 position,
            Transform parent,
            CancellationToken cancellationToken)
        {
            var pooled = Take(address);
            if (pooled != null)
            {
                Activate(pooled, position, parent);
                return pooled;
            }

            return await CreateAsync(address, position, parent, cancellationToken);
        }

        public void Return(GameObject instance)
        {
            var instanceId = instance != null ? instance.GetInstanceID() : 0;
            if (instance == null ||
                _availableIds.Contains(instanceId) ||
                !_addresses.TryGetValue(instanceId, out var address))
            {
                return;
            }

            PrepareForStorage(instance);
            GetAvailable(address).Push(instance);
            _availableIds.Add(instanceId);
        }

        public void Discard(GameObject instance)
        {
            if (instance == null)
            {
                return;
            }

            Forget(instance);
            _assetProvider.Release(instance);
        }

        public void Dispose()
        {
            ReleaseOwnedInstances();
            ClearTracking();
        }

        private GameObject Take(string address)
        {
            if (!_available.TryGetValue(address, out var instances))
            {
                return null;
            }

            return PopExisting(instances);
        }

        private GameObject PopExisting(Stack<GameObject> instances)
        {
            while (instances.Count > 0)
            {
                var instance = instances.Pop();
                if (instance != null)
                {
                    _availableIds.Remove(instance.GetInstanceID());
                    return instance;
                }
            }

            return null;
        }

        private async UniTask<GameObject> CreateAsync(
            string address,
            Vector3 position,
            Transform parent,
            CancellationToken cancellationToken)
        {
            var instance = await _assetProvider.InstantiateAsync(address, position, parent, cancellationToken);
            Track(address, instance);
            return instance;
        }

        private void Track(string address, GameObject instance)
        {
            _owned.Add(instance);
            _addresses.Add(instance.GetInstanceID(), address);
        }

        private void Activate(GameObject instance, Vector3 position, Transform parent)
        {
            instance.transform.SetParent(parent, false);
            instance.transform.SetPositionAndRotation(position, Quaternion.identity);
            instance.SetActive(true);
        }

        private void PrepareForStorage(GameObject instance)
        {
            instance.TryGetComponent<Animal>(out var animal);
            animal?.PrepareForPool();
            instance.SetActive(false);
        }

        private Stack<GameObject> GetAvailable(string address)
        {
            if (!_available.TryGetValue(address, out var instances))
            {
                instances = CreateAvailable(address);
            }

            return instances;
        }

        private Stack<GameObject> CreateAvailable(string address)
        {
            var instances = new Stack<GameObject>();
            _available.Add(address, instances);
            return instances;
        }

        private void Forget(GameObject instance)
        {
            var instanceId = instance.GetInstanceID();
            _owned.Remove(instance);
            _addresses.Remove(instanceId);
            _availableIds.Remove(instanceId);
        }

        private void ReleaseOwnedInstances()
        {
            foreach (var instance in _owned)
            {
                _assetProvider.Release(instance);
            }
        }

        private void ClearTracking()
        {
            _owned.Clear();
            _addresses.Clear();
            _availableIds.Clear();
            _available.Clear();
        }
    }
}
