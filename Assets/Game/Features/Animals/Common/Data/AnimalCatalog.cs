using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ZooWorld.Game.Features.Animals.Common.Data
{
    public sealed class AnimalCatalog
    {
        private readonly IAnimalConfigSource _source;
        private IReadOnlyList<AnimalConfigDto> _definitions = Array.Empty<AnimalConfigDto>();

        public AnimalCatalog(IAnimalConfigSource source)
        {
            _source = ValidateSource(source);
        }

        public async UniTask InitializeAsync(CancellationToken cancellationToken)
        {
            _definitions = await _source.LoadAsync(cancellationToken);
            EnsureNotEmpty();
        }

        public AnimalConfigDto GetRandom()
        {
            EnsureNotEmpty();
            return SelectWeighted();
        }

        private IAnimalConfigSource ValidateSource(IAnimalConfigSource source)
        {
            return source ?? throw new ArgumentNullException(nameof(source));
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

        private float PositiveWeight(AnimalConfigDto definition)
        {
            return Mathf.Max(0f, definition.SpawnWeight);
        }
    }
}
