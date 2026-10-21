using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using ZooWorld.Game.Features.Spawning;

namespace ZooWorld.Services.Assets
{
    public sealed class AddressableAnimalAssetProvider : IAnimalAssetProvider
    {
        public UniTask<GameObject> InstantiateAsync(
            string address,
            Vector3 position,
            Transform parent,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var operation = Addressables.InstantiateAsync(address, position, Quaternion.identity, parent);
            return new PendingInstantiation(operation, address, cancellationToken).Task;
        }

        public void Release(GameObject instance)
        {
            if (instance != null)
            {
                Addressables.ReleaseInstance(instance);
            }
        }

        private sealed class PendingInstantiation
        {
            private readonly UniTaskCompletionSource<GameObject> _completion =
                new UniTaskCompletionSource<GameObject>();
            private readonly string _address;
            private readonly CancellationToken _cancellationToken;
            private CancellationTokenRegistration _registration;

            public PendingInstantiation(
                AsyncOperationHandle<GameObject> operation,
                string address,
                CancellationToken cancellationToken)
            {
                _address = address;
                _cancellationToken = cancellationToken;
                operation.Completed += Complete;
                _registration = cancellationToken.Register(Cancel);
            }

            public UniTask<GameObject> Task => _completion.Task;

            private void Cancel()
            {
                _completion.TrySetCanceled(_cancellationToken);
            }

            private void Complete(AsyncOperationHandle<GameObject> handle)
            {
                _registration.Dispose();
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    CompleteSuccess(handle.Result);
                    return;
                }

                CompleteFailure(handle);
            }

            private void CompleteSuccess(GameObject instance)
            {
                if (_cancellationToken.IsCancellationRequested || !_completion.TrySetResult(instance))
                {
                    Addressables.ReleaseInstance(instance);
                }
            }

            private void CompleteFailure(AsyncOperationHandle<GameObject> handle)
            {
                ReleaseHandle(handle);
                _completion.TrySetException(
                    new InvalidOperationException($"Failed to load addressable animal prefab '{_address}'."));
            }

            private void ReleaseHandle(AsyncOperationHandle<GameObject> handle)
            {
                if (handle.IsValid())
                {
                    Addressables.Release(handle);
                }
            }
        }
    }
}
