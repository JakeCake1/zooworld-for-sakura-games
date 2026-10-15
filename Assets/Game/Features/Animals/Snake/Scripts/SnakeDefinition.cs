using UnityEngine;
using ZooWorld.Game.Features.Animals.Common.Data;

namespace ZooWorld.Game.Features.Animals.Snake
{
    [CreateAssetMenu(menuName = "Zoo World/Animals/Snake Definition", fileName = "SnakeDefinition")]
    public sealed class SnakeDefinition : AnimalDefinition
    {
        [SerializeField, Min(0.1f)] private float _speed = 2.1f;

        public override AnimalConfigDto ToDto()
        {
            return new SnakeConfigDto(Id, FeedingType, SpawnWeight, PrefabAddress, _speed);
        }
    }
}
