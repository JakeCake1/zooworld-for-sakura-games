using UnityEngine;

namespace ZooWorld.Game.Features.Animals.Common.Movement
{
    [CreateAssetMenu(menuName = "Zoo World/Movement/Jump", fileName = "JumpMovement")]
    public sealed class JumpMovementDefinition : MovementDefinition
    {
        [SerializeField, Min(0.1f)] private float _interval = 1.2f;
        [SerializeField, Min(0.1f)] private float _distance = 2.1f;
        [SerializeField, Min(0.05f)] private float _duration = 0.35f;

        public override MovementConfigDto ToDto()
        {
            return new JumpMovementConfigDto(_interval, _distance, _duration);
        }
    }
}
