using UnityEngine;
using ZooWorld.Domain;
using ZooWorld.Infrastructure;
using ZooWorld.Presentation;

namespace ZooWorld.Composition
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
            _assetProvider.InstantiateAsync(definition.PrefabAddress, position, _animalRoot, instance =>
            {
                if (!instance.TryGetComponent<AnimalActor>(out var actor))
                {
                    Debug.LogError($"Addressable '{definition.PrefabAddress}' has no AnimalActor component.");
                    _assetProvider.Release(instance);
                    return;
                }

                actor.Initialize(
                    ++_spawnSequence,
                    definition,
                    _foodChain,
                    _statistics,
                    _camera,
                    _assetProvider.Release);
            });
        }
    }
}
