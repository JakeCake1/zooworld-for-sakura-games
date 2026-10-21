using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ZooWorld.Game.Features.Spawning
{
    public interface IAnimalAssetProvider
    {
        UniTask<GameObject> InstantiateAsync(
            string address,
            Vector3 position,
            Transform parent,
            CancellationToken cancellationToken);

        void Release(GameObject instance);
    }
}
