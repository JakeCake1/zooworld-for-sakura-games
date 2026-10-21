using System;
using UnityEngine;

namespace ZooWorld.Game.Features.Animals.Common.Movement
{
    public interface IAnimalMovementBuilder
    {
        Type ConfigType { get; }
        IAnimalMovement Build(Rigidbody body, MovementConfigDto config);
    }
}
