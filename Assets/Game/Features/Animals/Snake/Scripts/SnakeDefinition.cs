using ZooWorld.Game.Features.Animals.Common.Data;
using ZooWorld.Game.Features.Animals.Common.Movement;
using UnityEngine;

namespace ZooWorld.Game.Features.Animals.Snake
{
    [CreateAssetMenu(menuName = "Zoo World/Animals/Snake Definition", fileName = "SnakeDefinition")]
    public sealed class SnakeDefinition : AnimalDefinition
    {
        protected override AnimalConfigDto CreateDto(MovementConfigDto movement)
        {
            return new SnakeConfigDto(Id, FeedingType, SpawnWeight, PrefabAddress, movement);
        }
    }
}
