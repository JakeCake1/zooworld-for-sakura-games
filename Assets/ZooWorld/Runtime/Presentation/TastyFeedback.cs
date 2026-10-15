using DG.Tweening;
using UnityEngine;

namespace ZooWorld.Presentation
{
    public sealed class TastyFeedback : MonoBehaviour
    {
        [SerializeField] private TextMesh _label;
        [SerializeField, Min(0.1f)] private float _duration = 0.8f;

        private Sequence _sequence;

        private void Awake()
        {
            if (_label != null)
            {
                _label.gameObject.SetActive(false);
            }
        }

        public void Show()
        {
            if (_label == null)
            {
                return;
            }

            _sequence?.Kill();
            _label.gameObject.SetActive(true);
            _label.transform.localScale = Vector3.one;
            var color = _label.color;
            color.a = 1f;
            _label.color = color;

            _sequence = DOTween.Sequence()
                .Append(_label.transform.DOPunchScale(Vector3.one * 0.35f, 0.25f))
                .AppendInterval(_duration * 0.5f)
                .Append(DOTween.To(
                    () => _label.color,
                    value => _label.color = value,
                    new Color(color.r, color.g, color.b, 0f),
                    _duration * 0.5f))
                .OnComplete(() => _label.gameObject.SetActive(false));
        }

        private void OnDisable()
        {
            _sequence?.Kill();
            _sequence = null;
        }
    }
}
