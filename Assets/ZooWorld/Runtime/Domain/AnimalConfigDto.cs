using System;

namespace ZooWorld.Domain
{
    /// <summary>
    /// Storage-agnostic animal data consumed by gameplay code.
    /// ScriptableObject, JSON, remote config, or a database can all map into this DTO.
    /// </summary>
    [Serializable]
    public sealed class AnimalConfigDto
    {
        public AnimalConfigDto(
            string id,
            AnimalDiet diet,
            MovementKind movementKind,
            float moveSpeed,
            float jumpInterval,
            float jumpDistance,
            float jumpDuration,
            float spawnWeight,
            string prefabAddress)
        {
            Id = id;
            Diet = diet;
            MovementKind = movementKind;
            MoveSpeed = moveSpeed;
            JumpInterval = jumpInterval;
            JumpDistance = jumpDistance;
            JumpDuration = jumpDuration;
            SpawnWeight = spawnWeight;
            PrefabAddress = prefabAddress;
        }

        public string Id { get; }
        public AnimalDiet Diet { get; }
        public MovementKind MovementKind { get; }
        public float MoveSpeed { get; }
        public float JumpInterval { get; }
        public float JumpDistance { get; }
        public float JumpDuration { get; }
        public float SpawnWeight { get; }
        public string PrefabAddress { get; }
    }
}
