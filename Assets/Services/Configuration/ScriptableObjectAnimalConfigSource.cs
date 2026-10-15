using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using ZooWorld.Game.Features.Animals.Common.Data;

namespace ZooWorld.Services.Configuration
{
    /// <summary>
    /// Unity authoring adapter. Replace this registration with JSON, remote config, or database DTO mapping.
    /// </summary>
    public sealed class ScriptableObjectAnimalConfigSource : IAnimalConfigSource
    {
        public const string AddressablesLabel = "animal-definition";

        public async UniTask<IReadOnlyList<AnimalConfigDto>> LoadAsync(CancellationToken cancellationToken)
        {
            var operation = Addressables.LoadAssetsAsync<AnimalDefinition>(AddressablesLabel, null);
            try
            {
                await WaitForCompletion(operation, cancellationToken);
                return CompleteLoad(operation);
            }
            finally
            {
                Release(operation);
            }
        }

        private async UniTask WaitForCompletion(
            AsyncOperationHandle<IList<AnimalDefinition>> operation,
            CancellationToken cancellationToken)
        {
            while (!operation.IsDone)
            {
                await UniTask.Yield(cancellationToken);
            }
        }

        private IReadOnlyList<AnimalConfigDto> CompleteLoad(
            AsyncOperationHandle<IList<AnimalDefinition>> operation)
        {
            if (operation.Status != AsyncOperationStatus.Succeeded)
            {
                throw new InvalidOperationException(
                    $"Failed to load animal definitions with label '{AddressablesLabel}'.");
            }

            return MapDefinitions(operation.Result);
        }

        private void Release(AsyncOperationHandle<IList<AnimalDefinition>> operation)
        {
            if (operation.IsValid())
            {
                Addressables.Release(operation);
            }
        }

        private IReadOnlyList<AnimalConfigDto> MapDefinitions(IList<AnimalDefinition> definitions)
        {
            var result = new AnimalConfigDto[definitions.Count];
            for (var index = 0; index < definitions.Count; index++)
            {
                result[index] = definitions[index].ToDto();
            }

            return result;
        }
    }
}
