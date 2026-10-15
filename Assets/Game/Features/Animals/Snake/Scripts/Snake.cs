using System;
using UnityEngine;
using ZooWorld.Game.Features.Animals.Common.Data;
using ZooWorld.Game.Features.Animals.Common.Movement;
using ZooWorld.Game.Features.Animals.Common.Presentation;

namespace ZooWorld.Game.Features.Animals.Snake
{
    public sealed class Snake : AnimalActor, IPredator
    {
        [SerializeField] private TastyFeedback _tastyFeedback;

        public void NotifyFed()
        {
            if (!IsDead)
            {
                _tastyFeedback?.Show();
            }
        }

        protected override IAnimalMovement CreateMovement(AnimalConfigDto definition, Rigidbody body)
        {
            var config = definition as SnakeConfigDto;
            return new SnakeMovement(body, config ?? throw WrongConfig(definition));
        }

        private static Exception WrongConfig(AnimalConfigDto definition)
        {
            return new ArgumentException($"Snake requires {nameof(SnakeConfigDto)}, received {definition?.GetType().Name}.");
        }
    }
}
