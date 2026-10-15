using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ZooWorld.Game.Features.Animals.Common.Movement
{
    public sealed class AnimalMovementRunner
    {
        private const float ViewportMargin = 0.03f;
        private readonly IAnimalMovement _movement;
        private readonly Transform _transform;
        private readonly Camera _camera;
        private readonly Func<bool> _canMove;

        public AnimalMovementRunner(
            IAnimalMovement movement,
            Transform transform,
            Camera camera,
            Func<bool> canMove)
        {
            _movement = movement;
            _transform = transform;
            _camera = camera;
            _canMove = canMove;
        }

        public async UniTask RunAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await UniTask.Yield(PlayerLoopTiming.FixedUpdate, cancellationToken);
                StepMovement();
            }
        }

        private void StepMovement()
        {
            if (!_canMove())
            {
                return;
            }

            RedirectAtBoundary();
            _movement.Step(Time.fixedDeltaTime);
        }

        private void RedirectAtBoundary()
        {
            if (_camera != null && !IsInsideCameraView())
            {
                _movement.Redirect(DirectionToWorldCenter());
            }
        }

        private bool IsInsideCameraView()
        {
            var viewport = _camera.WorldToViewportPoint(_transform.position);
            return viewport.x >= ViewportMargin && viewport.x <= 1f - ViewportMargin &&
                   viewport.y >= ViewportMargin && viewport.y <= 1f - ViewportMargin;
        }

        private Vector3 DirectionToWorldCenter()
        {
            return Vector3.ProjectOnPlane(-_transform.position, Vector3.up);
        }
    }
}
