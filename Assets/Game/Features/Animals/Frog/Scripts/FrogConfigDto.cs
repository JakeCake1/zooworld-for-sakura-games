using System;
using ZooWorld.Game.Features.Animals.Common.Data;

namespace ZooWorld.Game.Features.Animals.Frog
{
    [Serializable]
    public sealed class FrogConfigDto : AnimalConfigDto
    {
        public FrogConfigDto(
            string id,
            AnimalDiet diet,
            float spawnWeight,
            string prefabAddress,
            float jumpInterval,
            float jumpDistance,
            float jumpDuration)
            : base(id, diet, spawnWeight, prefabAddress)
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
