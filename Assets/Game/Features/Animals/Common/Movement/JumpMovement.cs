using DG.Tweening;
using UnityEngine;

namespace ZooWorld.Game.Features.Animals.Common.Movement
{
    public sealed class JumpMovement : IAnimalMovement
    {
        private readonly Rigidbody _body;
        private readonly float _interval;
        private readonly float _distance;
        private readonly float _duration;
        private Vector3 _direction;
        private float _timeUntilJump;
        private Tween _jumpTween;

        public JumpMovement(Rigidbody body, float interval, float distance, float duration)
        {
            _body = body;
            _interval = interval;
            _distance = distance;
            _duration = duration;
            ResetInitialState();
        }

        public void Step(float deltaTime)
        {
            if (IsJumping())
            {
                return;
            }

            CountDown(deltaTime);
            TryJump();
        }

        public void Redirect(Vector3 direction)
        {
            var flattened = Vector3.ProjectOnPlane(direction, Vector3.up);
            if (flattened.sqrMagnitude <= 0.001f)
            {
                return;
            }

            ApplyRedirect(flattened.normalized);
        }

        public void Stop()
        {
            KillJump();
            _body.linearVelocity = Vector3.zero;
        }

        private void ResetInitialState()
        {
            _direction = RandomDirection();
            _timeUntilJump = Random.Range(0f, _interval);
        }

        private bool IsJumping()
        {
            return _jumpTween != null && _jumpTween.IsActive();
        }

        private void CountDown(float deltaTime)
        {
            _timeUntilJump -= deltaTime;
        }

        private void TryJump()
        {
            if (_timeUntilJump > 0f)
            {
                return;
            }

            PrepareNextJump();
            StartJump();
        }

        private void PrepareNextJump()
        {
            _direction = RandomDirection();
            _timeUntilJump = _interval;
        }

        private void ApplyRedirect(Vector3 direction)
        {
            _direction = direction;
            _timeUntilJump = _interval;
            KillJump();
            StartJump();
        }

        private void StartJump()
        {
            _jumpTween = CreateJumpTween();
        }

        private Tween CreateJumpTween()
        {
            var target = _body.position + _direction * _distance;
            return DOTween
                .To(() => _body.position, position => _body.MovePosition(position), target, _duration)
                .SetEase(Ease.OutQuad)
                .SetUpdate(UpdateType.Fixed)
                .OnComplete(ClearJump);
        }

        private void KillJump()
        {
            _jumpTween?.Kill();
            ClearJump();
        }

        private void ClearJump()
        {
            _jumpTween = null;
        }

        private Vector3 RandomDirection()
        {
            var direction = Random.insideUnitCircle.normalized;
            return new Vector3(direction.x, 0f, direction.y);
        }
    }
}
