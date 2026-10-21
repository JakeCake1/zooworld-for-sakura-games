using System;
using ZooWorld.Game.Features.Animals.Common.Data;
using ZooWorld.Game.Features.Animals.Common.Movement;

namespace ZooWorld.Game.Features.Animals.Snake
{
    [Serializable]
    public sealed class SnakeConfigDto : AnimalConfigDto
    {
        public SnakeConfigDto(
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
