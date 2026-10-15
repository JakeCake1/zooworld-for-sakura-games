using System;
using System.Collections.Generic;
using UnityEngine;

namespace ZooWorld.Domain
{
    public sealed class AnimalCatalog
    {
        private readonly IReadOnlyList<AnimalConfigDto> _definitions;

        public AnimalCatalog(IAnimalConfigSource source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            _definitions = source.Load();
        }

        public AnimalConfigDto GetRandom()
        {
            if (_definitions.Count == 0)
            {
                throw new InvalidOperationException("Animal catalog is empty.");
            }

            var totalWeight = 0f;
            for (var index = 0; index < _definitions.Count; index++)
            {
                totalWeight += Mathf.Max(0f, _definitions[index].SpawnWeight);
            }

            if (totalWeight <= 0f)
            {
                return _definitions[UnityEngine.Random.Range(0, _definitions.Count)];
            }

            var roll = UnityEngine.Random.value * totalWeight;
            for (var index = 0; index < _definitions.Count; index++)
            {
                roll -= Mathf.Max(0f, _definitions[index].SpawnWeight);
                if (roll <= 0f)
                {
                    return _definitions[index];
                }
            }

            return _definitions[_definitions.Count - 1];
        }
    }
}
