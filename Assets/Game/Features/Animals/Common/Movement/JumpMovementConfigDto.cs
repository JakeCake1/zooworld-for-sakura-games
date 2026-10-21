using System;

namespace ZooWorld.Game.Features.Animals.Common.Movement
{
    [Serializable]
    public sealed class JumpMovementConfigDto : MovementConfigDto
    {
        public JumpMovementConfigDto(float interval, float distance, float duration)
        {
            Interval = interval;
            Distance = distance;
            Duration = duration;
        }

        public float Interval { get; }
        public float Distance { get; }
        public float Duration { get; }
    }
}
