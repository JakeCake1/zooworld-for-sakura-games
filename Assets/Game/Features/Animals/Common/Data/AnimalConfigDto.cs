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
            AnimalDiet diet,
            float spawnWeight,
            string prefabAddress)
        {
            Id = id;
            Diet = diet;
            SpawnWeight = spawnWeight;
            PrefabAddress = prefabAddress;
        }

        public string Id { get; }
        public AnimalDiet Diet { get; }
        public float SpawnWeight { get; }
        public string PrefabAddress { get; }
    }
}
