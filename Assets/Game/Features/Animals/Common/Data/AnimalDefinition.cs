using UnityEngine;
using UnityEngine.Serialization;

namespace ZooWorld.Game.Features.Animals.Common.Data
{
    public abstract class AnimalDefinition : ScriptableObject
    {
        [SerializeField] private string _id = "animal";
        [FormerlySerializedAs("_diet")]
        [SerializeField] private FeedingType _feedingType;
        [SerializeField, Min(0.01f)] private float _spawnWeight = 1f;
        [SerializeField] private string _prefabAddress = "animals/animal";

        protected string Id => _id;
        protected FeedingType FeedingType => _feedingType;
        protected float SpawnWeight => _spawnWeight;
        protected string PrefabAddress => _prefabAddress;

        public abstract AnimalConfigDto ToDto();
    }
}
