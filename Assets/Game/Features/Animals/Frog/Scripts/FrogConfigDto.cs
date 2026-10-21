using System;
using ZooWorld.Game.Features.Animals.Common.Data;
using ZooWorld.Game.Features.Animals.Common.Movement;

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
            MovementConfigDto movement)
            : base(id, feedingType, spawnWeight, prefabAddress, movement)
        {
        }
    }
}
