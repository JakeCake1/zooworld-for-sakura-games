using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using ZooWorld.Game.Features.Spawning;

namespace ZooWorld.Tests.EditMode
{
    public sealed class AnimalPoolTests
    {
        [Test]
        public async Task Return_MakesInstanceAvailableForReuse()
        {
            var provider = new FakeAnimalAssetProvider();
            var pool = new AnimalPool(provider);
            var parent = new GameObject("Pool root").transform;

            var first = await pool.RentAsync("animals/test", Vector3.zero, parent, CancellationToken.None);
            pool.Return(first);
            var second = await pool.RentAsync("animals/test", Vector3.one, parent, CancellationToken.None);

            Assert.That(second, Is.SameAs(first));
            Assert.That(provider.InstantiateCount, Is.EqualTo(1));
            pool.Dispose();
            Object.DestroyImmediate(parent.gameObject);
        }

        private sealed class FakeAnimalAssetProvider : IAnimalAssetProvider
        {
            public int InstantiateCount { get; private set; }

            public UniTask<GameObject> InstantiateAsync(
                string address,
                Vector3 position,
                Transform parent,
                CancellationToken cancellationToken)
            {
                InstantiateCount++;
                var instance = new GameObject(address);
                instance.transform.SetParent(parent);
                instance.transform.position = position;
                return UniTask.FromResult(instance);
            }

            public void Release(GameObject instance)
            {
                Object.DestroyImmediate(instance);
            }
        }
    }
}
