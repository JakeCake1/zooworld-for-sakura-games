using System;
using ZooWorld.Game.Features.Animals.Common.Data;

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
            float speed)
            : base(id, feedingType, spawnWeight, prefabAddress)
        {
            Speed = speed;
        }

        public float Speed { get; }
    }
}
