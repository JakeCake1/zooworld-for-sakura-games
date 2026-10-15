using System;
using System.Collections.Generic;
using ZooWorld.Domain;

namespace ZooWorld.Infrastructure
{
    /// <summary>
    /// Adapter from Unity authoring assets to storage-independent DTOs.
    /// Replace this registration with a JSON or remote implementation to change persistence.
    /// </summary>
    public sealed class ScriptableObjectAnimalConfigSource : IAnimalConfigSource
    {
        private readonly IReadOnlyList<AnimalDefinition> _definitions;

        public ScriptableObjectAnimalConfigSource(IReadOnlyList<AnimalDefinition> definitions)
        {
            _definitions = definitions ?? throw new ArgumentNullException(nameof(definitions));
        }

        public IReadOnlyList<AnimalConfigDto> Load()
        {
            var result = new AnimalConfigDto[_definitions.Count];
            for (var index = 0; index < _definitions.Count; index++)
            {
                result[index] = _definitions[index].ToDto();
            }

            return result;
        }
    }
}
