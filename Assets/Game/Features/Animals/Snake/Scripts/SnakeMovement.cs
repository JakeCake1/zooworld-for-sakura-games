using UnityEngine;
using ZooWorld.Game.Features.Animals.Common.Movement;

namespace ZooWorld.Game.Features.Animals.Snake
{
    public sealed class SnakeMovement : IAnimalMovement
    {
        private readonly Rigidbody _body;
        private readonly float _speed;
        private Vector3 _direction;

        public SnakeMovement(Rigidbody body, SnakeConfigDto config)
        {
            _body = body;
            _speed = config.Speed;
            _direction = RandomDirection();
        }

        public void Step(float deltaTime)
        {
            _body.linearVelocity = _direction * _speed;
        }

        public void Redirect(Vector3 direction)
        {
            var flattened = Vector3.ProjectOnPlane(direction, Vector3.up);
            if (flattened.sqrMagnitude > 0.001f)
            {
                _direction = flattened.normalized;
            }
        }

        public void Stop()
        {
            _body.linearVelocity = Vector3.zero;
        }

        private static Vector3 RandomDirection()
        {
            var direction = Random.insideUnitCircle.normalized;
            return new Vector3(direction.x, 0f, direction.y);
        }
    }
}
