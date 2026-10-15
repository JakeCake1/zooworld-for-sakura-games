using System.Collections.Generic;

namespace ZooWorld.Domain
{
    public interface IAnimalConfigSource
    {
        IReadOnlyList<AnimalConfigDto> Load();
    }
}
