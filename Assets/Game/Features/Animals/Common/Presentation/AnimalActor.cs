using System;
using UnityEngine;
using ZooWorld.Game.Features.Animals.Common.Data;
using ZooWorld.Game.Features.Animals.Common.Movement;
using ZooWorld.Game.Features.FoodChain;

namespace ZooWorld.Game.Features.Animals.Common.Presentation
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public abstract class AnimalActor : MonoBehaviour
    {
        private AnimalConfigDto _definition;
        private IAnimalMovement _movement;
        private FoodChainService _foodChain;
        private DeathStatistics _statistics;
        private Camera _camera;
        private Action<GameObject> _release;

        public AnimalDiet Diet => _definition.Diet;
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
            StoreCommonDependencies(spawnSequence, definition, foodChain, statistics, camera, release);
            _movement = CreateMovement(definition, GetComponent<Rigidbody>());
            RenameInstance();
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

        private void FixedUpdate()
        {
            if (!CanMove())
            {
                return;
            }

            RedirectAtBoundary();
            _movement.Tick(Time.fixedDeltaTime);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent<AnimalActor>(out var other))
            {
                _foodChain.TryResolve(this, other, Time.frameCount);
            }
        }

        private void StoreCommonDependencies(
            int spawnSequence,
            AnimalConfigDto definition,
            FoodChainService foodChain,
            DeathStatistics statistics,
            Camera camera,
            Action<GameObject> release)
        {
            SpawnSequence = spawnSequence;
            _definition = definition;
            _foodChain = foodChain;
            _statistics = statistics;
            _camera = camera;
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
            _movement.Stop();
            _statistics.Record(Diet);
        }

        private void ReleaseInstance()
        {
            _release(gameObject);
        }

        private void RedirectAtBoundary()
        {
            if (_camera == null || IsInsideCameraView())
            {
                return;
            }

            _movement.Redirect(DirectionToWorldCenter());
        }

        private bool IsInsideCameraView()
        {
            var viewport = _camera.WorldToViewportPoint(transform.position);
            const float margin = 0.03f;
            return viewport.x >= margin && viewport.x <= 1f - margin &&
                   viewport.y >= margin && viewport.y <= 1f - margin;
        }

        private Vector3 DirectionToWorldCenter()
        {
            return Vector3.ProjectOnPlane(-transform.position, Vector3.up);
        }
    }
}
