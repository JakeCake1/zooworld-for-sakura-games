using System;
using UnityEngine;

namespace ZooWorld.Game.Features.Animals.Common.Movement
{
    public sealed class JumpMovementBuilder : IAnimalMovementBuilder
    {
        public Type ConfigType => typeof(JumpMovementConfigDto);

        public IAnimalMovement Build(Rigidbody body, MovementConfigDto config)
        {
            return BuildJump(body, (JumpMovementConfigDto)config);
        }

        private IAnimalMovement BuildJump(Rigidbody body, JumpMovementConfigDto config)
        {
            return new JumpMovement(body, config.Interval, config.Distance, config.Duration);
        }
    }
}
