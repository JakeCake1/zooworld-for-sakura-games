using System;
using ZooWorld.Game.Features.Animals.Common.Movement;

namespace ZooWorld.Game.Features.Animals.Common.Data
{
    /// <summary>
    /// Storage-independent data shared by every animal. Species data belongs to derived DTOs.
    /// </summary>
    [Serializable]
    public abstract class AnimalConfigDto
    {
        protected AnimalConfigDto(
            string id,
            FeedingType feedingType,
            float spawnWeight,
            string prefabAddress,
            MovementConfigDto movement)
        {
            Id = id;
            FeedingType = feedingType;
            SpawnWeight = spawnWeight;
            PrefabAddress = prefabAddress;
            Movement = movement ?? throw new ArgumentNullException(nameof(movement));
        }

        public string Id { get; }
        public FeedingType FeedingType { get; }
        public float SpawnWeight { get; }
        public string PrefabAddress { get; }
        public MovementConfigDto Movement { get; }
    }
}
