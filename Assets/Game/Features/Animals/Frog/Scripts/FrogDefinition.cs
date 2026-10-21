using ZooWorld.Game.Features.Animals.Common.Data;
using ZooWorld.Game.Features.Animals.Common.Movement;
using UnityEngine;

namespace ZooWorld.Game.Features.Animals.Frog
{
    [CreateAssetMenu(menuName = "Zoo World/Animals/Frog Definition", fileName = "FrogDefinition")]
    public sealed class FrogDefinition : AnimalDefinition
    {
        protected override AnimalConfigDto CreateDto(MovementConfigDto movement)
        {
            return new FrogConfigDto(Id, FeedingType, SpawnWeight, PrefabAddress, movement);
        }
    }
}
