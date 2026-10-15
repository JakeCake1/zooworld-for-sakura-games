using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using ZooWorld.Game.Features.Animals.Common.Data;
using ZooWorld.Game.Features.Animals.Common.Movement;
using ZooWorld.Game.Features.FoodChain;

namespace ZooWorld.Game.Features.Animals.Common.Presentation
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public abstract class Animal : MonoBehaviour
    {
        private AnimalConfigDto _definition;
        private IAnimalMovement _movement;
        private CancellationTokenSource _movementCancellation;
        private FoodChainService _foodChain;
        private DeathStatistics _statistics;
        private Action<GameObject> _release;

        public FeedingType FeedingType => _definition.FeedingType;
        public int SpawnSequence { get; private set; }
        public bool IsDead { get; private set; }

        public void Initialize(
            int spawnSequence,
            AnimalConfigDto definition,
            FoodChainService foodChain,
            DeathStatistics statistics,
            Camera camera,
            Action<GameObject> release)
        {
            StoreCommonDependencies(spawnSequence, definition, foodChain, statistics, release);
            _movement = CreateMovement(definition, GetComponent<Rigidbody>());
            RenameInstance();
            StartMovement(camera);
        }

        public void Die()
        {
            if (IsDead)
            {
                return;
            }

            MarkDead();
            ReleaseInstance();
        }

        protected abstract IAnimalMovement CreateMovement(AnimalConfigDto definition, Rigidbody body);

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent<Animal>(out var other))
            {
                _foodChain.TryResolve(this, other, Time.frameCount);
            }
        }

        private void StoreCommonDependencies(
            int spawnSequence,
            AnimalConfigDto definition,
            FoodChainService foodChain,
            DeathStatistics statistics,
            Action<GameObject> release)
        {
            SpawnSequence = spawnSequence;
            _definition = definition;
            _foodChain = foodChain;
            _statistics = statistics;
            _release = release;
        }

        private void RenameInstance()
        {
            name = $"{_definition.Id} #{SpawnSequence}";
        }

        private bool CanMove()
        {
            return _movement != null && !IsDead;
        }

        private void MarkDead()
        {
            IsDead = true;
            CancelMovement();
            _movement.Stop();
            _statistics.Record(FeedingType);
        }

        private void ReleaseInstance()
        {
            _release(gameObject);
        }

        private void StartMovement(Camera camera)
        {
            _movementCancellation = CancellationTokenSource.CreateLinkedTokenSource(this.GetCancellationTokenOnDestroy());
            var runner = new AnimalMovementRunner(_movement, transform, camera, CanMove);
            runner.RunAsync(_movementCancellation.Token).SuppressCancellationThrow().Forget();
        }

        private void CancelMovement()
        {
            _movementCancellation?.Cancel();
            _movementCancellation?.Dispose();
            _movementCancellation = null;
        }

        private void OnDestroy()
        {
            CancelMovement();
        }
    }
}
