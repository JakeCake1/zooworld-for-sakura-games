using System;
using UnityEngine;
using ZooWorld.Game.Features.Animals.Common.Data;
using ZooWorld.Game.Features.Animals.Common.Movement;
using ZooWorld.Game.Features.Animals.Common.Presentation;
using ZooWorld.Game.Features.FoodChain;

namespace ZooWorld.Game.Features.Spawning
{
    public sealed class AnimalFactory
    {
        private readonly FoodChainService _foodChain;
        private readonly DeathStatistics _statistics;
        private readonly IAnimalAssetProvider _assetProvider;
        private readonly AnimalMovementFactory _movementFactory;
        private readonly Transform _animalRoot;
        private readonly Camera _camera;
        private int _spawnSequence;

        public AnimalFactory(
            FoodChainService foodChain,
            DeathStatistics statistics,
            IAnimalAssetProvider assetProvider,
            AnimalMovementFactory movementFactory,
            Transform animalRoot)
        {
            _foodChain = foodChain;
            _statistics = statistics;
            _assetProvider = assetProvider;
            _movementFactory = movementFactory;
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
            var animal = RequireAnimal(instance, definition.PrefabAddress);
            var movement = CreateMovement(instance, definition);
            InitializeAnimal(animal, definition, movement);
        }

        private Animal RequireAnimal(GameObject instance, string address)
        {
            if (instance.TryGetComponent<Animal>(out var animal))
            {
                return animal;
            }

            _assetProvider.Release(instance);
            throw new InvalidOperationException($"Addressable '{address}' has no Animal component.");
        }

        private IAnimalMovement CreateMovement(GameObject instance, AnimalConfigDto definition)
        {
            return _movementFactory.Create(instance.GetComponent<Rigidbody>(), definition.Movement);
        }

        private void InitializeAnimal(
            Animal animal,
            AnimalConfigDto definition,
            IAnimalMovement movement)
        {
            animal.Initialize(
                ++_spawnSequence,
                definition,
                movement,
                _foodChain,
                _statistics,
                _camera,
                _assetProvider.Release);
        }
    }
}
