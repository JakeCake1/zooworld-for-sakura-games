using System;
using ZooWorld.Game.Features.Animals.Common.Data;

namespace ZooWorld.Game.Features.Animals.Snake
{
    [Serializable]
    public sealed class SnakeConfigDto : AnimalConfigDto
    {
        public SnakeConfigDto(
            string id,
            AnimalDiet diet,
            float spawnWeight,
            string prefabAddress,
            float speed)
            : base(id, diet, spawnWeight, prefabAddress)
        {
            Speed = speed;
        }

        public float Speed { get; }
    }
}
