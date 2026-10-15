using UnityEngine;
using ZooWorld.Game.Features.Animals.Common.Data;

namespace ZooWorld.Game.Features.Animals.Frog
{
    [CreateAssetMenu(menuName = "Zoo World/Animals/Frog Definition", fileName = "FrogDefinition")]
    public sealed class FrogDefinition : AnimalDefinition
    {
        [SerializeField, Min(0.1f)] private float _jumpInterval = 1.2f;
        [SerializeField, Min(0.1f)] private float _jumpDistance = 2.1f;
        [SerializeField, Min(0.05f)] private float _jumpDuration = 0.35f;

        public override AnimalConfigDto ToDto()
        {
            return new FrogConfigDto(
                Id,
                Diet,
                SpawnWeight,
                PrefabAddress,
                _jumpInterval,
                _jumpDistance,
                _jumpDuration);
        }
    }
}
