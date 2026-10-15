namespace ZooWorld.Editor.AnimalScaffolding
{
    public sealed class AnimalScaffoldingTemplates
    {
        public string ConfigDto(string animalName)
        {
            return $@"using System;
using ZooWorld.Game.Features.Animals.Common.Data;

namespace ZooWorld.Game.Features.Animals.{animalName}
{{
    [Serializable]
    public sealed class {animalName}ConfigDto : AnimalConfigDto
    {{
        public {animalName}ConfigDto(string id, FeedingType feedingType, float spawnWeight, string prefabAddress, float speed)
            : base(id, feedingType, spawnWeight, prefabAddress)
        {{
            Speed = speed;
        }}

        public float Speed {{ get; }}
    }}
}}
";
        }

        public string Definition(string animalName)
        {
            return $@"using UnityEngine;
using ZooWorld.Game.Features.Animals.Common.Data;

namespace ZooWorld.Game.Features.Animals.{animalName}
{{
    [CreateAssetMenu(menuName = ""Zoo World/Animals/{animalName} Definition"", fileName = ""{animalName}Definition"")]
    public sealed class {animalName}Definition : AnimalDefinition
    {{
        [SerializeField, Min(0f)] private float _speed = 2f;

        public override AnimalConfigDto ToDto()
        {{
            return new {animalName}ConfigDto(Id, FeedingType, SpawnWeight, PrefabAddress, _speed);
        }}
    }}
}}
";
        }

        public string Animal(string animalName)
        {
            return $@"using System;
using UnityEngine;
using ZooWorld.Game.Features.Animals.Common.Data;
using ZooWorld.Game.Features.Animals.Common.Movement;
using ZooWorld.Game.Features.Animals.Common.Presentation;

namespace ZooWorld.Game.Features.Animals.{animalName}
{{
    public sealed class {animalName} : Animal
    {{
        protected override IAnimalMovement CreateMovement(AnimalConfigDto definition, Rigidbody body)
        {{
            return CreateLinearMovement(body, RequireConfig(definition));
        }}

        private IAnimalMovement CreateLinearMovement(Rigidbody body, {animalName}ConfigDto config)
        {{
            return new LinearMovement(body, config.Speed);
        }}

        private {animalName}ConfigDto RequireConfig(AnimalConfigDto definition)
        {{
            return definition as {animalName}ConfigDto ?? throw WrongConfig(definition);
        }}

        private Exception WrongConfig(AnimalConfigDto definition)
        {{
            return new ArgumentException($""{animalName} requires {animalName}ConfigDto, received {{definition?.GetType().Name}}."");
        }}
    }}
}}
";
        }
    }
}
