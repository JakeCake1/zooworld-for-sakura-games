using UnityEngine;
using ZooWorld.Game.Features.Animals.Common.Data;
using ZooWorld.Game.Features.Animals.Common.Presentation;
using ZooWorld.Game.Features.FoodChain;

namespace ZooWorld.Game.Features.Spawning
{
    public sealed class AnimalFactory
    {
        private readonly FoodChainService _foodChain;
        private readonly DeathStatistics _statistics;
        private readonly IAnimalAssetProvider _assetProvider;
        private readonly Transform _animalRoot;
        private readonly Camera _camera;
        private int _spawnSequence;

        public AnimalFactory(
            FoodChainService foodChain,
            DeathStatistics statistics,
            IAnimalAssetProvider assetProvider,
            Transform animalRoot)
        {
            _foodChain = foodChain;
            _statistics = statistics;
            _assetProvider = assetProvider;
            _animalRoot = animalRoot;
            _camera = Camera.main;
        }

        public void Create(AnimalConfigDto definition, Vector3 position)
        {
            _assetProvider.InstantiateAsync(
                definition.PrefabAddress,
                position,
                _animalRoot,
                instance => InitializeInstance(instance, definition));
        }

        private void InitializeInstance(GameObject instance, AnimalConfigDto definition)
        {
            if (!instance.TryGetComponent<Animal>(out var animal))
            {
                RejectInvalidPrefab(instance, definition.PrefabAddress);
                return;
            }

            InitializeAnimal(animal, definition);
        }

        private void InitializeAnimal(Animal animal, AnimalConfigDto definition)
        {
            animal.Initialize(
                ++_spawnSequence,
                definition,
                _foodChain,
                _statistics,
                _camera,
                _assetProvider.Release);
        }

        private void RejectInvalidPrefab(GameObject instance, string address)
        {
            Debug.LogError($"Addressable '{address}' has no Animal component.");
            _assetProvider.Release(instance);
        }
    }
}
