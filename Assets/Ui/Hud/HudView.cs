using UnityEngine;
using UnityEngine.UI;
using VContainer;
using ZooWorld.Game.Features.FoodChain;

namespace ZooWorld.Ui.Hud
{
    public sealed class HudView : MonoBehaviour
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
            Subscribe();
            Refresh();
        }

        private void OnDestroy()
        {
            Unsubscribe();
        }

        private void Subscribe()
        {
            _statistics.Changed += Refresh;
        }

        private void Unsubscribe()
        {
            if (_statistics != null)
            {
                _statistics.Changed -= Refresh;
            }
        }

        private void Refresh()
        {
            RefreshPreyDeaths();
            RefreshPredatorDeaths();
        }

        private void RefreshPreyDeaths()
        {
            _preyDeaths.text = $"Dead prey: {_statistics.PreyDeaths}";
        }

        private void RefreshPredatorDeaths()
        {
            _predatorDeaths.text = $"Dead predators: {_statistics.PredatorDeaths}";
        }
    }
}
