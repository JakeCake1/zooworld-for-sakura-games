using UnityEngine;
using UnityEngine.Serialization;
using ZooWorld.Game.Features.Animals.Common.Movement;

namespace ZooWorld.Game.Features.Animals.Common.Data
{
    public abstract class AnimalDefinition : ScriptableObject
    {
        [SerializeField] private string _id = "animal";
        [FormerlySerializedAs("_diet")]
        [SerializeField] private FeedingType _feedingType;
        [SerializeField, Min(0.01f)] private float _spawnWeight = 1f;
        [SerializeField] private string _prefabAddress = "animals/animal";
        [SerializeField] private MovementDefinition _movement;

        protected string Id => _id;
        protected FeedingType FeedingType => _feedingType;
        protected float SpawnWeight => _spawnWeight;
        protected string PrefabAddress => _prefabAddress;

        public AnimalConfigDto ToDto()
        {
            return CreateDto(RequireMovement().ToDto());
        }

        protected abstract AnimalConfigDto CreateDto(MovementConfigDto movement);

        private MovementDefinition RequireMovement()
        {
            return _movement != null
                ? _movement
                : throw new MissingReferenceException($"Movement is not configured for '{name}'.");
        }
    }
}
