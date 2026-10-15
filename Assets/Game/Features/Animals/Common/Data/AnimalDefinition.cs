using UnityEngine;

namespace ZooWorld.Game.Features.Animals.Common.Data
{
    public abstract class AnimalDefinition : ScriptableObject
    {
        [SerializeField] private string _id = "animal";
        [SerializeField] private AnimalDiet _diet;
        [SerializeField, Min(0.01f)] private float _spawnWeight = 1f;
        [SerializeField] private string _prefabAddress = "animals/animal";

        protected string Id => _id;
        protected AnimalDiet Diet => _diet;
        protected float SpawnWeight => _spawnWeight;
        protected string PrefabAddress => _prefabAddress;

        public abstract AnimalConfigDto ToDto();
    }
}
