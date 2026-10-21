using System;
using UnityEngine;

namespace ZooWorld.Game.Features.Animals.Common.Movement
{
    public sealed class LinearMovementBuilder : IAnimalMovementBuilder
    {
        public Type ConfigType => typeof(LinearMovementConfigDto);

        public IAnimalMovement Build(Rigidbody body, MovementConfigDto config)
        {
            return BuildLinear(body, (LinearMovementConfigDto)config);
        }

        private IAnimalMovement BuildLinear(Rigidbody body, LinearMovementConfigDto config)
        {
            return new LinearMovement(body, config.Speed);
        }
    }
}
