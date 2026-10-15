using DG.Tweening;
using UnityEngine;

namespace ZooWorld.Movement
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
            _direction = RandomDirection();
            _timeUntilJump = Random.Range(0f, interval);
        }

        public void Tick(float deltaTime)
        {
            if (_jumpTween != null && _jumpTween.IsActive())
            {
                return;
            }

            _timeUntilJump -= deltaTime;
            if (_timeUntilJump > 0f)
            {
                return;
            }

            _direction = RandomDirection();
            _timeUntilJump = _interval + _duration;
            StartJump();
        }

        public void Redirect(Vector3 direction)
        {
            var flattened = Vector3.ProjectOnPlane(direction, Vector3.up);
            if (flattened.sqrMagnitude > 0.001f)
            {
                _direction = flattened.normalized;
                _timeUntilJump = _interval + _duration;
                _jumpTween?.Kill();
                StartJump();
            }
        }

        public void Stop()
        {
            _jumpTween?.Kill();
            _jumpTween = null;
            _body.linearVelocity = Vector3.zero;
        }

        private void StartJump()
        {
            var target = _body.position + _direction * _distance;
            _jumpTween = DOTween
                .To(() => _body.position, position => _body.MovePosition(position), target, _duration)
                .SetEase(Ease.OutQuad)
                .SetUpdate(UpdateType.Fixed)
                .OnComplete(() => _jumpTween = null);
        }

        private static Vector3 RandomDirection()
        {
            var direction = Random.insideUnitCircle.normalized;
            return new Vector3(direction.x, 0f, direction.y);
        }
    }
}
