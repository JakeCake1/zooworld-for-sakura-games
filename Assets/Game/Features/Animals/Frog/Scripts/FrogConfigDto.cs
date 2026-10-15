using System;
using ZooWorld.Game.Features.Animals.Common.Data;

namespace ZooWorld.Game.Features.Animals.Frog
{
    [Serializable]
    public sealed class FrogConfigDto : AnimalConfigDto
    {
        public FrogConfigDto(
            string id,
            FeedingType feedingType,
            float spawnWeight,
            string prefabAddress,
            float jumpInterval,
            float jumpDistance,
            float jumpDuration)
            : base(id, feedingType, spawnWeight, prefabAddress)
        {
            JumpInterval = jumpInterval;
            JumpDistance = jumpDistance;
            JumpDuration = jumpDuration;
        }

        public float JumpInterval { get; }
        public float JumpDistance { get; }
        public float JumpDuration { get; }
    }
}
