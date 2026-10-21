using System;
using System.Collections.Generic;
using UnityEngine;

namespace ZooWorld.Game.Features.Animals.Common.Movement
{
    public sealed class AnimalMovementFactory
    {
        private readonly Dictionary<Type, IAnimalMovementBuilder> _builders =
            new Dictionary<Type, IAnimalMovementBuilder>();

        public AnimalMovementFactory(IReadOnlyList<IAnimalMovementBuilder> builders)
        {
            Register(builders);
        }

        public IAnimalMovement Create(Rigidbody body, MovementConfigDto config)
        {
            return GetBuilder(config).Build(body, config);
        }

        private void Register(IReadOnlyList<IAnimalMovementBuilder> builders)
        {
            for (var index = 0; index < builders.Count; index++)
            {
                _builders.Add(builders[index].ConfigType, builders[index]);
            }
        }

        private IAnimalMovementBuilder GetBuilder(MovementConfigDto config)
        {
            if (config != null && _builders.TryGetValue(config.GetType(), out var builder))
            {
                return builder;
            }

            throw new NotSupportedException($"Movement config '{config?.GetType().Name ?? "null"}' is not registered.");
        }
    }
}
