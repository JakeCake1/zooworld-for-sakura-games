using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using ZooWorld.Game.Features.Animals.Common.Data;

namespace ZooWorld.Game.Features.Spawning
{
    public sealed class AnimalSpawner : MonoBehaviour
    {
        [SerializeField] private Vector2 _spawnExtents = new Vector2(7f, 4.5f);
        [SerializeField] private Vector2 _spawnIntervalRange = new Vector2(1f, 2f);
        [SerializeField] private float _spawnHeight = 0.6f;
        private AnimalFactory _factory;
        private AnimalCatalog _catalog;

        [Inject]
        public void Construct(AnimalFactory factory, AnimalCatalog catalog)
        {
            _factory = factory;
            _catalog = catalog;
        }

        private void Start()
        {
            RunAsync(this.GetCancellationTokenOnDestroy()).Forget();
        }

        private async UniTask RunAsync(CancellationToken cancellationToken)
        {
            await _catalog.InitializeAsync(cancellationToken);
            while (!cancellationToken.IsCancellationRequested)
            {
                await WaitForNextSpawn(cancellationToken);
                await SpawnAnimalAsync(cancellationToken);
            }
        }

        private UniTask WaitForNextSpawn(CancellationToken cancellationToken)
        {
            var seconds = UnityEngine.Random.Range(_spawnIntervalRange.x, _spawnIntervalRange.y);
            return UniTask.Delay(TimeSpan.FromSeconds(seconds), cancellationToken: cancellationToken);
        }

        private UniTask SpawnAnimalAsync(CancellationToken cancellationToken)
        {
            return _factory.CreateAsync(_catalog.GetRandom(), RandomPosition(), cancellationToken);
        }

        private Vector3 RandomPosition()
        {
            return new Vector3(
                UnityEngine.Random.Range(-_spawnExtents.x, _spawnExtents.x),
                _spawnHeight,
                UnityEngine.Random.Range(-_spawnExtents.y, _spawnExtents.y));
        }
    }
}
