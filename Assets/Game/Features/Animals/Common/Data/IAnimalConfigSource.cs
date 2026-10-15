using System.Collections.Generic;

namespace ZooWorld.Game.Features.Animals.Common.Data
{
    public interface IAnimalConfigSource
    {
        IReadOnlyList<AnimalConfigDto> Load();
    }
}
