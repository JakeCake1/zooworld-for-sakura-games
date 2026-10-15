using System;
using UnityEngine;
using ZooWorld.Game.Features.Animals.Common.Data;
using ZooWorld.Game.Features.Animals.Common.Movement;
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

        protected override IAnimalMovement CreateMovement(AnimalConfigDto definition, Rigidbody body)
        {
            return CreateLinearMovement(body, RequireConfig(definition));
        }

        private IAnimalMovement CreateLinearMovement(Rigidbody body, SnakeConfigDto config)
        {
            return new LinearMovement(body, config.Speed);
        }

        private SnakeConfigDto RequireConfig(AnimalConfigDto definition)
        {
            return definition as SnakeConfigDto ?? throw WrongConfig(definition);
        }

        private Exception WrongConfig(AnimalConfigDto definition)
        {
            return new ArgumentException($"Snake requires {nameof(SnakeConfigDto)}, received {definition?.GetType().Name}.");
        }
    }
}
