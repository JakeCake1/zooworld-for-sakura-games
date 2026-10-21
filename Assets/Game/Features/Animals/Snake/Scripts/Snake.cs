using UnityEngine;
using ZooWorld.Game.Features.Animals.Common.Presentation;

namespace ZooWorld.Game.Features.Animals.Snake
{
    public sealed class Snake : Animal, IPredator
    {
        [SerializeField] private TastyFeedback _tastyFeedback;

        public void NotifyFed()
        {
            if (!IsDead)
            {
                _tastyFeedback?.Show();
            }
        }
    }
}
