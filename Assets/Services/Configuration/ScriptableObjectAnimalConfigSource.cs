using System;
using System.Collections.Generic;
using ZooWorld.Game.Features.Animals.Common.Data;

namespace ZooWorld.Services.Configuration
{
    /// <summary>
    /// Unity authoring adapter. Replace this registration with JSON, remote config, or database DTO mapping.
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
