using System.Collections;
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

        private IEnumerator Start()
        {
            while (enabled)
            {
                yield return WaitForNextSpawn();
                SpawnAnimal();
            }
        }

        private WaitForSeconds WaitForNextSpawn()
        {
            return new WaitForSeconds(Random.Range(_spawnIntervalRange.x, _spawnIntervalRange.y));
        }

        private void SpawnAnimal()
        {
            _factory.Create(_catalog.GetRandom(), RandomPosition());
        }

        private Vector3 RandomPosition()
        {
            return new Vector3(
                Random.Range(-_spawnExtents.x, _spawnExtents.x),
                _spawnHeight,
                Random.Range(-_spawnExtents.y, _spawnExtents.y));
        }
    }
}
