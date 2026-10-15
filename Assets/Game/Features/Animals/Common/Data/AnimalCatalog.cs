using System;
using System.Collections.Generic;
using UnityEngine;

namespace ZooWorld.Game.Features.Animals.Common.Data
{
    public sealed class AnimalCatalog
    {
        private readonly IReadOnlyList<AnimalConfigDto> _definitions;

        public AnimalCatalog(IAnimalConfigSource source)
        {
            _definitions = LoadDefinitions(source);
        }

        public AnimalConfigDto GetRandom()
        {
            EnsureNotEmpty();
            return SelectWeighted();
        }

        private static IReadOnlyList<AnimalConfigDto> LoadDefinitions(IAnimalConfigSource source)
        {
            return source?.Load() ?? throw new ArgumentNullException(nameof(source));
        }

        private void EnsureNotEmpty()
        {
            if (_definitions.Count == 0)
            {
                throw new InvalidOperationException("Animal catalog is empty.");
            }
        }

        private AnimalConfigDto SelectWeighted()
        {
            var totalWeight = CalculateTotalWeight();
            return totalWeight > 0f ? SelectByRoll(totalWeight) : SelectUniformly();
        }

        private float CalculateTotalWeight()
        {
            var totalWeight = 0f;
            for (var index = 0; index < _definitions.Count; index++)
            {
                totalWeight += PositiveWeight(_definitions[index]);
            }

            return totalWeight;
        }

        private AnimalConfigDto SelectByRoll(float totalWeight)
        {
            var roll = UnityEngine.Random.value * totalWeight;
            for (var index = 0; index < _definitions.Count; index++)
            {
                roll -= PositiveWeight(_definitions[index]);
                if (roll <= 0f)
                {
                    return _definitions[index];
                }
            }

            return _definitions[_definitions.Count - 1];
        }

        private AnimalConfigDto SelectUniformly()
        {
            return _definitions[UnityEngine.Random.Range(0, _definitions.Count)];
        }

        private static float PositiveWeight(AnimalConfigDto definition)
        {
            return Mathf.Max(0f, definition.SpawnWeight);
        }
    }
}
