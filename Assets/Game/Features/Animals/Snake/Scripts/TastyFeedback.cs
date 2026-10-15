using DG.Tweening;
using UnityEngine;

namespace ZooWorld.Game.Features.Animals.Snake
{
    public sealed class TastyFeedback : MonoBehaviour
    {
        [SerializeField] private TextMesh _label;
        [SerializeField, Min(0.1f)] private float _duration = 0.8f;
        private Sequence _sequence;

        private void Awake()
        {
            SetLabelActive(false);
        }

        public void Show()
        {
            if (_label == null)
            {
                return;
            }

            ResetVisualState();
            _sequence = CreateSequence();
        }

        private void OnDisable()
        {
            KillSequence();
        }

        private void ResetVisualState()
        {
            KillSequence();
            SetLabelActive(true);
            _label.transform.localScale = Vector3.one;
            _label.color = OpaqueLabelColor();
        }

        private Sequence CreateSequence()
        {
            return DOTween.Sequence()
                .Append(_label.transform.DOPunchScale(Vector3.one * 0.35f, 0.25f))
                .AppendInterval(_duration * 0.5f)
                .Append(CreateFadeTween())
                .OnComplete(() => SetLabelActive(false));
        }

        private Tween CreateFadeTween()
        {
            var color = OpaqueLabelColor();
            var transparent = new Color(color.r, color.g, color.b, 0f);
            return DOTween.To(() => _label.color, value => _label.color = value, transparent, _duration * 0.5f);
        }

        private Color OpaqueLabelColor()
        {
            var color = _label.color;
            color.a = 1f;
            return color;
        }

        private void KillSequence()
        {
            _sequence?.Kill();
            _sequence = null;
        }

        private void SetLabelActive(bool isActive)
        {
            if (_label != null)
            {
                _label.gameObject.SetActive(isActive);
            }
        }
    }
}
