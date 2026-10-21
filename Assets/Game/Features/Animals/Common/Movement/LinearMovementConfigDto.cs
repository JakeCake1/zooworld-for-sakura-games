using System;

namespace ZooWorld.Game.Features.Animals.Common.Movement
{
    [Serializable]
    public sealed class LinearMovementConfigDto : MovementConfigDto
    {
        public LinearMovementConfigDto(float speed)
        {
            Speed = speed;
        }

        public float Speed { get; }
    }
}
