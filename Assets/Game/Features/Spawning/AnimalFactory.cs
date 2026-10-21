using System;
using System.Threading;
using Cysharp.Threading.Tasks;
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
        private readonly AnimalPool _pool;
        private readonly AnimalMovementFactory _movementFactory;
        private readonly Transform _animalRoot;
        private readonly Camera _camera;
        private int _spawnSequence;

        public AnimalFactory(
            FoodChainService foodChain,
            DeathStatistics statistics,
            AnimalPool pool,
            AnimalMovementFactory movementFactory,
            Transform animalRoot,
            Camera camera)
        {
            _foodChain = foodChain;
            _statistics = statistics;
            _pool = pool;
            _movementFactory = movementFactory;
            _animalRoot = animalRoot;
            _camera = camera;
        }

        public async UniTask CreateAsync(
            AnimalConfigDto definition,
            Vector3 position,
            CancellationToken cancellationToken)
        {
            GameObject instance = null;
            try
            {
                instance = await _pool.RentAsync(
                    definition.PrefabAddress,
                    position,
                    _animalRoot,
                    cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                InitializeInstance(instance, definition);
            }
            catch (OperationCanceledException)
            {
                Discard(instance);
                throw;
            }
            catch (Exception exception)
            {
                Discard(instance);
                Debug.LogException(exception);
            }
        }

        private void InitializeInstance(GameObject instance, AnimalConfigDto definition)
        {
            var animal = RequireAnimal(instance, definition.PrefabAddress);
            var movement = CreateMovement(instance, definition);
            InitializeAnimal(animal, definition, movement);
        }

        private Animal RequireAnimal(GameObject instance, string address)
        {
            return instance.TryGetComponent<Animal>(out var animal)
                ? animal
                : throw new InvalidOperationException($"Addressable '{address}' has no Animal component.");
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
                _pool.Return);
        }

        private void Discard(GameObject instance)
        {
            _pool.Discard(instance);
        }
    }
}
