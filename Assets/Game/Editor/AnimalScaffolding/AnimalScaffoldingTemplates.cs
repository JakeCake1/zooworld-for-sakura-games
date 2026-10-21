namespace ZooWorld.Editor.AnimalScaffolding
{
    public sealed class AnimalScaffoldingTemplates
    {
        public string ConfigDto(string animalName)
        {
            return $@"using System;
using ZooWorld.Game.Features.Animals.Common.Data;
using ZooWorld.Game.Features.Animals.Common.Movement;

namespace ZooWorld.Game.Features.Animals.{animalName}
{{
    [Serializable]
    public sealed class {animalName}ConfigDto : AnimalConfigDto
    {{
        public {animalName}ConfigDto(string id, FeedingType feedingType, float spawnWeight, string prefabAddress, MovementConfigDto movement)
            : base(id, feedingType, spawnWeight, prefabAddress, movement)
        {{
        }}
    }}
}}
";
        }

        public string Definition(string animalName)
        {
            return $@"using UnityEngine;
using ZooWorld.Game.Features.Animals.Common.Data;
using ZooWorld.Game.Features.Animals.Common.Movement;

namespace ZooWorld.Game.Features.Animals.{animalName}
{{
    [CreateAssetMenu(menuName = ""Zoo World/Animals/{animalName} Definition"", fileName = ""{animalName}Definition"")]
    public sealed class {animalName}Definition : AnimalDefinition
    {{
        protected override AnimalConfigDto CreateDto(MovementConfigDto movement)
        {{
            return new {animalName}ConfigDto(Id, FeedingType, SpawnWeight, PrefabAddress, movement);
        }}
    }}
}}
";
        }

        public string Animal(string animalName)
        {
            return $@"using ZooWorld.Game.Features.Animals.Common.Presentation;

namespace ZooWorld.Game.Features.Animals.{animalName}
{{
    public sealed class {animalName} : Animal
    {{
    }}
}}
";
        }
    }
}
