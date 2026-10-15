namespace ZooWorld.Editor.AnimalScaffolding
{
    internal static class AnimalScaffoldingTemplates
    {
        public static string ConfigDto(string animalName)
        {
            return $@"using System;
using ZooWorld.Game.Features.Animals.Common.Data;

namespace ZooWorld.Game.Features.Animals.{animalName}
{{
    [Serializable]
    public sealed class {animalName}ConfigDto : AnimalConfigDto
    {{
        public {animalName}ConfigDto(string id, AnimalDiet diet, float spawnWeight, string prefabAddress)
            : base(id, diet, spawnWeight, prefabAddress)
        {{
        }}
    }}
}}
";
        }

        public static string Definition(string animalName)
        {
            return $@"using ZooWorld.Game.Features.Animals.Common.Data;

namespace ZooWorld.Game.Features.Animals.{animalName}
{{
    public sealed class {animalName}Definition : AnimalDefinition
    {{
        public override AnimalConfigDto ToDto()
        {{
            return new {animalName}ConfigDto(Id, Diet, SpawnWeight, PrefabAddress);
        }}
    }}
}}
";
        }

        public static string Movement(string animalName)
        {
            return $@"using UnityEngine;
using ZooWorld.Game.Features.Animals.Common.Movement;

namespace ZooWorld.Game.Features.Animals.{animalName}
{{
    public sealed class {animalName}Movement : IAnimalMovement
    {{
        private readonly Rigidbody _body;

        public {animalName}Movement(Rigidbody body, {animalName}ConfigDto config)
        {{
            _body = body;
        }}

        public void Tick(float deltaTime)
        {{
            // Add species-specific movement.
        }}

        public void Redirect(Vector3 direction)
        {{
            // Add species-specific redirection.
        }}

        public void Stop()
        {{
            _body.linearVelocity = Vector3.zero;
        }}
    }}
}}
";
        }

        public static string Actor(string animalName)
        {
            return $@"using System;
using UnityEngine;
using ZooWorld.Game.Features.Animals.Common.Data;
using ZooWorld.Game.Features.Animals.Common.Movement;
using ZooWorld.Game.Features.Animals.Common.Presentation;

namespace ZooWorld.Game.Features.Animals.{animalName}
{{
    public sealed class {animalName} : AnimalActor
    {{
        protected override IAnimalMovement CreateMovement(AnimalConfigDto definition, Rigidbody body)
        {{
            var config = definition as {animalName}ConfigDto;
            return new {animalName}Movement(body, config ?? throw WrongConfig(definition));
        }}

        private static Exception WrongConfig(AnimalConfigDto definition)
        {{
            return new ArgumentException($""{animalName} requires {animalName}ConfigDto, received {{definition?.GetType().Name}}."");
        }}
    }}
}}
";
        }
    }
}
