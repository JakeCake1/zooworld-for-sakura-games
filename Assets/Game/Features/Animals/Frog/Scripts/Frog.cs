using System;
using UnityEngine;
using ZooWorld.Game.Features.Animals.Common.Data;
using ZooWorld.Game.Features.Animals.Common.Movement;
using ZooWorld.Game.Features.Animals.Common.Presentation;

namespace ZooWorld.Game.Features.Animals.Frog
{
    public sealed class Frog : AnimalActor
    {
        protected override IAnimalMovement CreateMovement(AnimalConfigDto definition, Rigidbody body)
        {
            var config = definition as FrogConfigDto;
            return new FrogMovement(body, config ?? throw WrongConfig(definition));
        }

        private static Exception WrongConfig(AnimalConfigDto definition)
        {
            return new ArgumentException($"Frog requires {nameof(FrogConfigDto)}, received {definition?.GetType().Name}.");
        }
    }
}
