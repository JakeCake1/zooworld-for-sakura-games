using UnityEngine;
using UnityEngine.UI;
using VContainer;
using ZooWorld.Domain;

namespace ZooWorld.Composition
{
    public sealed class HudPresenter : MonoBehaviour
    {
        [SerializeField] private Text _preyDeaths;
        [SerializeField] private Text _predatorDeaths;

        private DeathStatistics _statistics;

        [Inject]
        public void Construct(DeathStatistics statistics)
        {
            _statistics = statistics;
        }

        private void Start()
        {
            _statistics.Changed += Refresh;
            Refresh();
        }

        private void OnDestroy()
        {
            if (_statistics != null)
            {
                _statistics.Changed -= Refresh;
            }
        }

        private void Refresh()
        {
            _preyDeaths.text = $"Dead prey: {_statistics.PreyDeaths}";
            _predatorDeaths.text = $"Dead predators: {_statistics.PredatorDeaths}";
        }
    }
}
