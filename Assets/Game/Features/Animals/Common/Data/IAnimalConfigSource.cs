using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace ZooWorld.Game.Features.Animals.Common.Data
{
    public interface IAnimalConfigSource
    {
        UniTask<IReadOnlyList<AnimalConfigDto>> LoadAsync(CancellationToken cancellationToken);
    }
}
