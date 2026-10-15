using System;
using UnityEngine;
using ZooWorld.Domain;
using ZooWorld.Infrastructure;
using ZooWorld.Movement;

namespace ZooWorld.Presentation
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public sealed class AnimalActor : MonoBehaviour
    {
        [SerializeField] private TastyFeedback _tastyFeedback;

        private Rigidbody _body;
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
            SpawnSequence = spawnSequence;
            _definition = definition;
            _foodChain = foodChain;
            _statistics = statistics;
            _camera = camera;
            _release = release;
            _body = GetComponent<Rigidbody>();
            _movement = CreateMovement(definition);
            name = $"{definition.Id} #{spawnSequence}";
        }

        private void FixedUpdate()
        {
            if (_movement == null || IsDead)
            {
                return;
            }

            KeepInsideCameraView();
            _movement.Tick(Time.fixedDeltaTime);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent<AnimalActor>(out var other))
            {
                _foodChain.TryResolve(this, other, Time.frameCount);
            }
        }

        public void Die()
        {
            if (IsDead)
            {
                return;
            }

            IsDead = true;
            _movement.Stop();
            _statistics.Record(Diet);
            _release(gameObject);
        }

        public void NotifyFed()
        {
            if (!IsDead && Diet == AnimalDiet.Predator && _tastyFeedback != null)
            {
                _tastyFeedback.Show();
            }
        }

        private IAnimalMovement CreateMovement(AnimalConfigDto definition)
        {
            switch (definition.MovementKind)
            {
                case MovementKind.Jump:
                    return new JumpMovement(
                        _body,
                        definition.JumpInterval,
                        definition.JumpDistance,
                        definition.JumpDuration);
                default:
                    return new LinearMovement(_body, definition.MoveSpeed);
            }
        }

        private void KeepInsideCameraView()
        {
            if (_camera == null)
            {
                return;
            }

            var viewport = _camera.WorldToViewportPoint(transform.position);
            const float margin = 0.03f;
            if (viewport.x >= margin && viewport.x <= 1f - margin &&
                viewport.y >= margin && viewport.y <= 1f - margin)
            {
                return;
            }

            var toCenter = Vector3.ProjectOnPlane(-transform.position, Vector3.up);
            _movement.Redirect(toCenter);
        }
    }
}
