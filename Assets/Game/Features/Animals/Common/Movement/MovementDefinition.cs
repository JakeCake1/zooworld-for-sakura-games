using UnityEngine;

namespace ZooWorld.Game.Features.Animals.Common.Movement
{
    public abstract class MovementDefinition : ScriptableObject
    {
        public abstract MovementConfigDto ToDto();
    }
}
