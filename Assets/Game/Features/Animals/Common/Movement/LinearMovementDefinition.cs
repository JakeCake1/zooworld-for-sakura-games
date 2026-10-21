using UnityEngine;

namespace ZooWorld.Game.Features.Animals.Common.Movement
{
    [CreateAssetMenu(menuName = "Zoo World/Movement/Linear", fileName = "LinearMovement")]
    public sealed class LinearMovementDefinition : MovementDefinition
    {
        [SerializeField, Min(0f)] private float _speed = 2f;

        public override MovementConfigDto ToDto()
        {
            return new LinearMovementConfigDto(_speed);
        }
    }
}
