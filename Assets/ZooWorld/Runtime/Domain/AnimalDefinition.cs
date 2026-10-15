using UnityEngine;

namespace ZooWorld.Domain
{
    [CreateAssetMenu(menuName = "Zoo World/Animal Definition", fileName = "AnimalDefinition")]
    public sealed class AnimalDefinition : ScriptableObject
    {
        [SerializeField] private string _id = "animal";
        [SerializeField] private AnimalDiet _diet;
        [SerializeField] private MovementKind _movementKind;
        [SerializeField, Min(0.1f)] private float _moveSpeed = 2f;
        [SerializeField, Min(0.1f)] private float _jumpInterval = 1f;
        [SerializeField, Min(0.1f)] private float _jumpDistance = 2f;
        [SerializeField, Min(0.05f)] private float _jumpDuration = 0.3f;
        [SerializeField, Min(0.01f)] private float _spawnWeight = 1f;
        [SerializeField] private string _prefabAddress = "animals/animal";

        public string Id => _id;
        public AnimalDiet Diet => _diet;
        public MovementKind MovementKind => _movementKind;
        public float MoveSpeed => _moveSpeed;
        public float JumpInterval => _jumpInterval;
        public float JumpDistance => _jumpDistance;
        public float JumpDuration => _jumpDuration;
        public float SpawnWeight => _spawnWeight;
        public string PrefabAddress => _prefabAddress;

        public AnimalConfigDto ToDto()
        {
            return new AnimalConfigDto(
                _id,
                _diet,
                _movementKind,
                _moveSpeed,
                _jumpInterval,
                _jumpDistance,
                _jumpDuration,
                _spawnWeight,
                _prefabAddress);
        }
    }
}
