using System;

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
            string prefabAddress)
        {
            Id = id;
            FeedingType = feedingType;
            SpawnWeight = spawnWeight;
            PrefabAddress = prefabAddress;
        }

        public string Id { get; }
        public FeedingType FeedingType { get; }
        public float SpawnWeight { get; }
        public string PrefabAddress { get; }
    }
}
