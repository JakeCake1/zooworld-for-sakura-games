using System.Collections;
using UnityEngine;
using VContainer;
using ZooWorld.Domain;

namespace ZooWorld.Composition
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
                yield return new WaitForSeconds(Random.Range(
                    _spawnIntervalRange.x,
                    _spawnIntervalRange.y));

                var position = new Vector3(
                    Random.Range(-_spawnExtents.x, _spawnExtents.x),
                    _spawnHeight,
                    Random.Range(-_spawnExtents.y, _spawnExtents.y));
                _factory.Create(_catalog.GetRandom(), position);
            }
        }
    }
}
